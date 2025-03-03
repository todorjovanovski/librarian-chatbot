import os
from dotenv import load_dotenv, find_dotenv
from pinecone import Pinecone
from langchain import PromptTemplate
from langchain.chains import LLMChain
from pinecone_utils import get_vector_store
from models import get_llm_model
load_dotenv(find_dotenv(), override=True)
pc = Pinecone(
        api_key=os.environ.get("PINECONE_API_KEY")
    )
index = pc.Index("books")

def get_similarity_by_query(query, book_id, k=3):
    vector_store = get_vector_store()
    result = vector_store.similarity_search(query, k=k, filter={"book-id": book_id})
    return result

def get_prompt_template(title):
    prompt = PromptTemplate(
        template=f"""You have read {title}.
        Based on the context below, answer only the question asked.
        If you can't find the answer in the context, say that you don't have any knowledge:\n\nContext: {{context}}\n\nQuestion: {{question}}""",
        input_variables=["context", "question"],
    )
    return prompt

def get_generated_text(query, title, book_id, k=3):
    llm = get_llm_model()
    prompt = get_prompt_template(title)
    qa_chain = LLMChain(prompt=prompt, llm=llm)
    similarities = get_similarity_by_query(query, book_id, k)
    context = "\n".join([similarity.page_content for similarity in similarities])
    answer = qa_chain.run({"context": context, "question": query})
    return answer
