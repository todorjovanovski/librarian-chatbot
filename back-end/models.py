from langchain_huggingface import HuggingFaceEmbeddings, HuggingFaceEndpoint
from dotenv import load_dotenv, find_dotenv
import torch
load_dotenv(find_dotenv(), override=True)
def get_embeddings_model(model_name="sentence-transformers/all-mpnet-base-v2", model_kwargs={'device': 'cpu'}, encode_kwargs = {'normalize_embeddings': False}):
    device = "cuda" if torch.cuda.is_available() else "cpu"
    model_kwargs["device"] = device
    embedding_model = HuggingFaceEmbeddings(
        model_name=model_name,
        model_kwargs=model_kwargs,
        encode_kwargs=encode_kwargs
    )
    return embedding_model

def get_llm_model(repo_id="deepseek-ai/DeepSeek-R1-Distill-Qwen-32B", task="text-generation", max_new_tokens=1024, do_sample=False, repetition_penalty=1.03):
    llm = HuggingFaceEndpoint(
        timeout=600,
        repo_id=repo_id,
        task=task,
        max_new_tokens=max_new_tokens,
        do_sample=do_sample,
        repetition_penalty=repetition_penalty,
    )
    return llm
