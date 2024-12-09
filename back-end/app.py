import os
from fastapi import FastAPI, File, UploadFile, HTTPException
from fastapi.responses import JSONResponse
from tempfile import NamedTemporaryFile
import uvicorn

app = FastAPI()

@app.get("/hi")
async def hi():
    return {"message": "hi"}


if __name__ == "__main__":
    uvicorn.run(app, host="127.0.0.1", port=8000)