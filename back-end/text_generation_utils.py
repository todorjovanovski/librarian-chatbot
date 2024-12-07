import os
from dotenv import load_dotenv, find_dotenv
from pinecone import Pinecone
load_dotenv(find_dotenv(), override=True)
from pinecone_utils import get_vector_store
pc = Pinecone(
        api_key=os.environ.get("PINECONE_API_KEY")
    )
index = pc.Index("books")

def get_similarity_by_query(query, file, k=3):
    vector_store = get_vector_store()
    result = vector_store.similarity_search(query, k=k, filter={"source": file})
    return result

def get_prompt_template():
    prompt = PromptTemplate(
        template="""
        Based on the context below, answer only the question asked.
        If you can't find the answer in the context, say that you don't have any knowledge:\n\nContext: {context}\n\nQuestion: {question}\n\nAnswer:""",
        input_variables=["context", "question"],
    )
    return prompt
