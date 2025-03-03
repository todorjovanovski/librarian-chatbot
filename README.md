# Librarian - Mobile AI-Powered Book Assistant

## Project Overview
**Librarian** is a mobile application designed to enhance the reading experience by allowing users to upload books (or any document) in PDF format and ask questions about the content. The main feature is a chatbot that provides answers, helping users clarify complex sections and improve comprehension.

This project was developed as part of the **Mobile Information Systems** course.

## Features
- Upload and store PDF documents.
- Ask the chatbot questions about the uploaded document.
- Receive context-aware answers based on document content.
- View and manage past chats.
- Speech-to-text support for hands-free question input.
- Persist chat history using a local database.

## Technologies Used

### Back-End Technologies:
- **FastAPI** - API development and request handling.
- **LangChain** - LLM orchestration and processing.
- **HuggingFace** - Pre-trained AI models for embedding and response generation.
- **Pinecone** - Vector database for similarity search.

### Mobile Application Technologies:
- **.NET MAUI (C#)** - Cross-platform mobile application framework.
- **SQLite** - Local storage for chat history and document records.

---

## How It Works

### Document Upload & Processing
1. The user uploads a **PDF** document.
2. The PDF is converted into a **single string**.
3. The text is split into **chunks**, each being embedded to a vector using a HuggingFace embedding model.
5. The generated vectors are stored in **Pinecone**.
6. The user is redirected to the appropriate chat room.

### Asking Questions
1. A **Large Language Model (LLM)** from HuggingFace (e.g., Mistral) is used.
2. A **prompt template** is applied to refine the AI response.
3. The book identifier is used to **retrieve relevant content** from Pinecone using **cosine similarity**.
4. The **four most relevant vectors** are extracted and provided as context to the LLM.
5. The LLM analyzes the context and generates a response.
6. The user receives the answer within the chat interface.

### Chat & Data Management
- **Chats are stored** in SQLite for persistence.
- Users can **delete a chat** by swiping it from the chat list.
- The last interaction for each chat is displayed on the chat list.
- If a chat has no interaction, an empty message screen is shown.
- When deleting a chat, the book identifier is sent to the API, and all related vectors are removed from the database.

### Speech-to-Text Functionality
- Users can dictate questions instead of typing.
- If they change their mind, they can reset or switch back to the keyboard input.

---

## Mobile Application Workflow

### Adding a New Document
1. The user enters a **document name** and uploads a **PDF**.
2. The document can be **canceled** before starting a new chat.
3. Once the user starts a chat, it is **saved in SQLite** and opens in a new screen.

### Asking a Question
1. The user types a question and clicks **send**.
2. The system processes the query and returns an **AI-generated answer**.
3. The question and answer are **saved in SQLite**.
4. Users can also use **speech-to-text** for question input.
5. If the user changes their mind, they can **reset** the input field.

### Managing Chats
- Users can **navigate to previous chats** from the list.
- Swiping a chat **deletes it**.
- Clicking a chat opens it, displaying past interactions.

---

## Future Improvements
- Implementing **real-time synchronization** across devices.
- Supporting **additional document formats** (e.g., EPUB, DOCX).
- Enhancing **UI/UX** for a better user experience.
- Adding **multi-language support**.

---

## Installation & Setup
### Prerequisites
- .NET 8 SDK
- MAUI workloads
- Python 3.10+
- FastAPI, LangChain, HuggingFace, Pinecone libraries
- Env. variables: PINECONE_API_KEY, PINECONE_ENV, HUGGINGFACEHUB_API_TOKEN

### Running the Project
1. **Backend Setup:**
   ```sh
   pip install -r requirements.txt
   python.exe -m uvicorn app:app --reload
   ```
2. **Mobile App Setup:**
   ```sh
   dotnet build
   dotnet run
   (ios example) ~ dotnet build -t:Run -f net8.0-ios -p:RuntimeIdentifier=ios-arm64 -p:_DeviceName=MY_SPECIFIC_UDID
   ```
3. Upload a document and start interacting!

---

## License
This project is open-source and available under the **MIT License**.

