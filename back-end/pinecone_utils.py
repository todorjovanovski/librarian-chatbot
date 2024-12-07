from langchain.vectorstores import Pinecone as LangChainPinecone
from models import get_embeddings_model
from chunk_utils import get_chunks_for_file
import uuid
import os
def get_vector_store(index_name="books"):
    embedding_model = get_embeddings_model()
    vector_store = LangChainPinecone.from_existing_index(index_name, embedding_model)
    return vector_store

def get_vectorized_file(file):
    chunks = get_chunks_for_file(file)
    embedding_model = get_embeddings_model()
    vectors = [
    (
        str(uuid.uuid4()),
        embedding_model.embed_query(chunk.page_content),
        {"page": chunk.metadata.get("page", -1), "source": file, "text": chunk.page_content}
    )
    for chunk in chunks
]
    return vectors

def file_exists_in_database(file, index):
    query_response = index.query(
        vector=[0] * 1536,
        top_k=1,
        filter={"source": file}
    )
    return len(query_response["matches"]) > 0

def add_file_to_database(file, batch_size=100):
    from dotenv import load_dotenv, find_dotenv
    from pinecone import Pinecone
    load_dotenv(find_dotenv(), override=True)
    pc = Pinecone(
        api_key=os.environ.get("PINECONE_API_KEY")
    )
    index = pc.Index(name="books")

    if not file_exists_in_database(file, index):
        vectors = get_vectorized_file(file)
        for i in range(0, len(vectors), batch_size):
            batch = vectors[i:i + batch_size]
            index.upsert(batch)
    else:
        return file
