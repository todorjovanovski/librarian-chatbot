import os
from fastapi import FastAPI, File, UploadFile, HTTPException, Form
from fastapi.responses import JSONResponse
from tempfile import NamedTemporaryFile
import uvicorn
from pinecone_utils import add_file_to_database

app = FastAPI()

@app.post("/upload_book")
async def upload_pdf(pdf: UploadFile = File(...), title: str = Form(...)):
    if not pdf.filename.endswith(".pdf"):
        raise HTTPException(status_code=400, detail="Books must be uploaded only in PDF format.")
    
    with NamedTemporaryFile(delete=False, suffix=".pdf") as temp_file:
        temp_file.write(await pdf.read())
        temp_file.flush()
        temp_path = temp_file.name
        add_file_to_database(file=temp_path)
    os.remove(temp_path)

    return JSONResponse(content={"title":title})


if __name__ == "__main__":
    uvicorn.run(app, host="127.0.0.1", port=8000)