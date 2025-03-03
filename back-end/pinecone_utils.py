from langchain.vectorstores import Pinecone as LangChainPinecone
from models import get_embeddings_model
from chunk_utils import get_chunks_for_file
import uuid
import os
from dotenv import load_dotenv, find_dotenv
from pinecone import Pinecone
import asyncio

load_dotenv(find_dotenv(), override=True)
pc = Pinecone(api_key=os.environ.get("PINECONE_API_KEY"))
index = pc.Index(name="books")

def get_vector_store(index_name="books"):
    embedding_model = get_embeddings_model()
    vector_store = LangChainPinecone.from_existing_index(index_name, embedding_model)
    return vector_store

def get_vectorized_file(file, title, book_id):
    chunks = get_chunks_for_file(file, chunk_size=1024)
    embedding_model = get_embeddings_model()
    vectors = [
    (
        str(uuid.uuid4()),
        embedding_model.embed_query(chunk.page_content),
        {"page": chunk.metadata.get("page", -1), "source": title, "book-id": book_id, "text": chunk.page_content}
    )
    for chunk in chunks
]
    return vectors

def file_exists_in_database(book_id):
    query_response = index.query(
        vector=[0] * 768,
        top_k=10000,
        filter={"book-id": book_id}
    )
    return len(query_response["matches"]) > 0, query_response

async def add_file_to_database(file, title, batch_size=250):
    book_id = str(uuid.uuid4())
    vectors = get_vectorized_file(file, title, book_id)
    for i in range(0, len(vectors), batch_size):
        batch = vectors[i:i + batch_size]
        await asyncio.to_thread(index.upsert, batch) 
    return book_id

async def delete_vector_database(chat_id, batch_size=250):
    _, query_response = file_exists_in_database(chat_id)
    vector_ids = [match["id"] for match in query_response["matches"]]
    for i in range(0, len(vector_ids), batch_size):
        batch_vector_ids = vector_ids[i:i + batch_size]
        await asyncio.to_thread(index.delete, ids=batch_vector_ids)
