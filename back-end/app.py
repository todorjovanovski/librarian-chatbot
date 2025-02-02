import os
from fastapi import FastAPI, File, UploadFile, HTTPException, Form
from fastapi.responses import JSONResponse
from tempfile import NamedTemporaryFile
import uvicorn
from pinecone_utils import add_file_to_database, delete_vector_database, file_exists_in_database
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
        book_id = await add_file_to_database(file=temp_path,title=title)
    os.remove(temp_path)

    return JSONResponse(content={"Id": book_id, "Title":title})

@app.post("/ask")
async def ask_question(question: str = Form(...), title: str = Form(...), book_id: str = Form(...)):
    answer = get_generated_text(query=question, title=title, book_id=book_id, k=4)
    return JSONResponse(content={"Question": question, "Answer":answer})

@app.delete("/delete_chat/{chat_id}")
async def delete_chat(chat_id: str):
    await delete_vector_database(chat_id)
    return JSONResponse(
        content={"deleted": not file_exists_in_database(chat_id)[0]}
    )


if __name__ == "__main__":
    uvicorn.run(app, host="<YOUR_CURRENT_IP_ADDRESS>", port=8000)