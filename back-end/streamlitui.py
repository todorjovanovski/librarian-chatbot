import streamlit as st
import requests

# Set up the FastAPI endpoints
UPLOAD_ENDPOINT = "http://127.0.0.1:8000/upload_book"
ASK_ENDPOINT = "http://127.0.0.1:8000/ask"

# Page configuration
st.set_page_config(
    page_title="Chatbot with PDF Upload",
    layout="wide",
)

# CSS styling
css = '''
<style>
.chat-message {
    padding: 1.5rem; border-radius: 0.5rem; margin-bottom: 1rem; display: flex
}
.chat-message.user {
    background-color: #2b313e
}
.chat-message.bot {
    background-color: #475063
}
.chat-message .avatar {
  width: 20%;
}
.chat-message .avatar img {
  max-width: 78px;
  max-height: 78px;
  border-radius: 50%;
  object-fit: cover;
}
.chat-message .message {
  width: 80%;
  padding: 0 1.5rem;
  color: #fff;
}
</style>
'''
st.markdown(css, unsafe_allow_html=True)

# HTML templates
bot_template = '''
<div class="chat-message bot">
    <div class="avatar">
        <img src="https://img.freepik.com/free-vector/cartoon-style-robot-vectorart_78370-4103.jpg" style="max-height: 80px;">
    </div>
    <div class="message">{{MSG}}</div>
</div>
'''

user_template = '''
<div class="chat-message user">
    <div class="avatar">
        <img src="https://st4.depositphotos.com/3538469/23947/v/450/depositphotos_239471270-stock-illustration-connection-icon-user-sign.jpg" style="max-height: 80px;">
    </div>    
    <div class="message">{{MSG}}</div>
</div>
'''

# Sidebar: Upload PDF
st.sidebar.header("Upload PDF")
uploaded_file = st.sidebar.file_uploader("Choose a PDF file", type=["pdf"])

if st.sidebar.button("Upload"):
    if uploaded_file:
        try:
            files = {"book": (uploaded_file.name, uploaded_file, "application/pdf")}
            data = {"title": ""}
            response = requests.post(UPLOAD_ENDPOINT, files=files, data=data)
            if response.status_code == 200:
                st.sidebar.success(f"Uploaded file successfully.")
                
                # Store the book ID
                st.session_state["book_id"] = response.json()["book-id"]

                # Clear chat history on new upload
                st.session_state.chat_history = []
            else:
                st.sidebar.error(response.json().get("detail", "Error during upload"))
        except Exception as e:
            st.sidebar.error(f"Error: {e}")
    else:
        st.sidebar.warning("Please upload a PDF and provide a title.")

# Initialize session state variables
if "chat_history" not in st.session_state:
    st.session_state.chat_history = []

# Main Chat Interface
st.header("Bitzer Documentation Knowledge")

# Display chat history
for message in st.session_state.chat_history:
    if message["user"]:
        st.markdown(user_template.replace("{{MSG}}", message["text"]), unsafe_allow_html=True)
    else:
        st.markdown(bot_template.replace("{{MSG}}", message["text"]), unsafe_allow_html=True)

# Input box for questions
question = st.text_input("Ask a question about the uploaded Bitzer PDF File :")

if st.button("Send"):
    if question and "book_id" in st.session_state:
        # Append user question to chat history
        st.session_state.chat_history.append({"user": True, "text": question})

        with st.spinner("Fetching response..."):
            try:
                # Send the question to FastAPI
                data = {
                    "question": question,
                    "title": "",
                    "book_id": st.session_state["book_id"],
                }
                response = requests.post(ASK_ENDPOINT, data=data)
                if response.status_code == 200:
                    answer = response.json()["answer"]

                    # Append bot's response to chat history
                    st.session_state.chat_history.append({"user": False, "text": answer})
                else:
                    st.session_state.chat_history.append(
                        {"user": False, "text": "Error fetching the response."}
                    )
            except Exception as e:
                st.session_state.chat_history.append(
                    {"user": False, "text": f"Error: {e}"}
                )

        # Re-render the chat history
        # st.experimental_rerun()
        st.rerun()
    else:
        st.warning("Please upload a PDF and provide a title first.")
