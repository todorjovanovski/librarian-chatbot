import os
from langchain.document_loaders import PyPDFLoader
from langchain.text_splitter import RecursiveCharacterTextSplitter
def load_document(file):
    name, extension = os.path.splitext(file)
    if extension == ".pdf":
        print(f"Loading {file}")
        loader = PyPDFLoader(file)
    else:
        print("Document format is not suppoerted!")
        return None
    data = loader.load()
    return data

def chunk_data(data, chunk_size=256):
    text_splitter = RecursiveCharacterTextSplitter(chunk_size=chunk_size, chunk_overlap=0)
    chunks = text_splitter.split_documents(data)
    return chunks

def get_chunks_for_file(file, chunk_size=256):
    document = load_document(file)
    chunks = chunk_data(document, chunk_size)
    return chunks
