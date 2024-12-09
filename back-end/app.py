import os
from fastapi import FastAPI, File, UploadFile, HTTPException, Form
from fastapi.responses import JSONResponse
from tempfile import NamedTemporaryFile
import uvicorn
from pinecone_utils import add_file_to_database
from text_generation_utils import get_generated_text
app = FastAPI()

@app.post("/upload_book")
async def upload_book(book: UploadFile = File(...), title: str = Form(...)):
    if not book.filename.endswith(".pdf"):
        raise HTTPException(status_code=400, detail="Books must be uploaded only in PDF format.")
    
    with NamedTemporaryFile(delete=False, suffix=".pdf") as temp_file:
        temp_file.write(await book.read())
        temp_file.flush()
        temp_path = temp_file.name
        add_file_to_database(file=temp_path,title=title)
    os.remove(temp_path)

    return JSONResponse(content={"title":title})

@app.post("/ask")
async def ask_question(question: str = Form(...), title: str = Form(...)):
    answer = get_generated_text(query=question, title=title)
    return JSONResponse(content={"answer":answer})

if __name__ == "__main__":
    uvicorn.run(app, host="127.0.0.1", port=8000)