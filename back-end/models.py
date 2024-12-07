from langchain_huggingface import HuggingFaceEmbeddings, HuggingFaceEndpoint
def get_embeddings_model(model_name="sentence-transformers/all-mpnet-base-v2", model_kwargs={'device': 'cpu'}, encode_kwargs = {'normalize_embeddings': False}):
    device = "cuda" if torch.cuda.is_available() else "cpu"
    model_kwargs["device"] = device
    embedding_model = HuggingFaceEmbeddings(
        model_name=model_name,
        model_kwargs=model_kwargs,
        encode_kwargs=encode_kwargs
    )
    return embedding_model

def get_llm_model(repo_id="microsoft/Phi-3-mini-4k-instruct", task="text-generation", max_new_tokens=512, do_sample=False, repetition_penalty=1.03):
    llm = HuggingFaceEndpoint(
        repo_id=repo_id,
        task=task,
        max_new_tokens=max_new_tokens,
        do_sample=do_sample,
        repetition_penalty=repetition_penalty,
    )
    return llm
