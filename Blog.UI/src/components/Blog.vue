<template>
  <div class="blogs">
    <div class="blogs-header">
      <h2>Blogs</h2>
      <button @click="openAddForm" class="btn btn-primary">
        + Add Blog
      </button>
    </div>
    
    <table class="table">
      <thead>
      <tr>
        <th>ID</th>
        <th>Title</th>
        <th>Content</th>
        <th>Author</th>
        <th>Actions</th>
      </tr>
      </thead>
      <tbody>
      <tr v-for="blog in blogs" :key="blog.blogId">
        <td>{{ blog.blogId }}</td>
        <td>{{ blog.blogTitle }}</td>
        <td>{{ blog.blogContent }}</td>
        <td>{{ blog.author.authorName }}</td>
        <td>
          <button class="btn btn-warning btn-sm" @click="openUpdateForm(blog)">
            Update
          </button>
          <button class="btn btn-danger btn-sm" @click="deleteBlog(blog.blogId)">
            Delete
          </button>
        </td>
      </tr>
      </tbody>
    </table>

    
    <div v-if="showForm" class="form-overlay">
      <div class="form-card">
        <h3>{{ isUpdate ? "Update Blog" : "Add Blog" }}</h3>
        <form @submit.prevent="saveBlog">
          <input
              v-model="form.blogTitle"
              type="text"
              placeholder="Title"
              required
          />
          <textarea
              v-model="form.blogContent"
              placeholder="Content"
              required
          ></textarea>
          <label for="">Select Author</label>
          <select v-model="form.authorId" required>
            <option value="" disabled>Select Author</option>
            <option
                v-for="author in authors"
                :key="author.authorId"
                :value="author.authorId"
            >
              {{ author.authorName }}
            </option>
          </select>

          <div class="form-actions">
            <button type="submit" class="btn btn-success">Save</button>
            <button
                type="button"
                class="btn btn-secondary"
                @click="closeForm"
            >
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";
import { useAuthStore } from "../stores/auth";

const BLOG_API_URL = "http://localhost:5058/api/blogs";
const AUTHOR_API_URL = "http://localhost:5058/api/authors";

const blogs = ref([]);
const authors = ref([]);
const showForm = ref(false);
const isUpdate = ref(false);
const form = ref({ blogTitle: "", blogContent: "", authorId: null });

const authStore = useAuthStore();

const api = axios.create({
  headers: {
    Authorization: `Bearer ${authStore.accessToken}`,
  },
});

async function fetchBlogs() {
  try {
    const res = await api.get(BLOG_API_URL);
    blogs.value = res.data.data;
  } catch (err) {
    console.error("Error fetching blogs:", err);
  }
}

async function fetchAuthors() {
  try {
    const res = await api.get(AUTHOR_API_URL);
    authors.value = res.data.data;
  } catch (err) {
    console.error("Error fetching authors:", err);
  }
}

function openAddForm() {
  form.value = { blogTitle: "", blogContent: "", authorId: null };
  isUpdate.value = false;
  showForm.value = true;
}

function openUpdateForm(blog) {
  form.value = { ...blog };
  isUpdate.value = true;
  showForm.value = true;
}

function closeForm() {
  showForm.value = false;
}

async function saveBlog() {
  try {
    if (isUpdate.value) {
      await api.patch(`${BLOG_API_URL}/${form.value.blogId}`, form.value);
    } else {
      await api.post(BLOG_API_URL, form.value);
    }
    await fetchBlogs();
    closeForm();
  } catch (err) {
    console.error("Error saving blog:", err);
  }
}

async function deleteBlog(id) {
  if (confirm("Are you sure you want to delete this blog?")) {
    try {
      await api.delete(`${BLOG_API_URL}/${id}`);
      await fetchBlogs();
    } catch (err) {
      console.error("Error deleting blog:", err);
    }
  }
}

onMounted(() => {
  fetchAuthors();
  fetchBlogs();
});
</script>

<style scoped>
.blogs {
  max-width: 1000px;
  margin: auto;
  padding: 20px;
}

.blogs-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.blogs-header h2 {
  color: #2c3e50;
}

.table {
  width: 100%;
  margin-top: 20px;
  border-collapse: collapse;
  background: #fff;
  border-radius: 8px;
  overflow: hidden;
}

.table th {
  background: #2c3e50;
  color: white;
  padding: 12px;
  text-align: left;
}

.table td {
  border: 1px solid #ddd;
  padding: 10px;
}

.table tr:nth-child(even) {
  background: #f9f9f9;
}

.btn {
  padding: 6px 12px;
  border-radius: 6px;
  border: none;
  cursor: pointer;
  font-size: 0.9rem;
  margin: 0 10px;
}

.btn-primary {
  background:#2c3e50;
  color: white;
}

.btn-primary:hover {
  background: #2c3e50;
}

.btn-warning {
  background: #f39c12;
  color: white;
}

.btn-warning:hover {
  background: #d68910;
}

.btn-danger {
  background: #e74c3c;
  color: white;
}

.btn-danger:hover {
  background: #c0392b;
}

.btn-success {
  background: #27ae60;
  color: white;
}

.btn-success:hover {
  background: #1e8449;
}

.btn-secondary {
  background: #7f8c8d;
  color: white;
}

.btn-secondary:hover {
  background: #626e70;
}


.form-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.form-card {
  background: white;
  padding: 2rem;
  border-radius: 12px;
  width: 400px;
  max-width: 90%;
  box-shadow: 0 6px 18px rgba(0, 0, 0, 0.2);
}

.form-card h3 {
  margin-bottom: 1rem;
  color: #2c3e50;
  text-align: center;
}

form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

input,
textarea,
select {
  padding: 10px;
  border-radius: 6px;
  border: 1px solid #ddd;
  font-size: 1rem;
  width: 100%;
}

textarea {
  min-height: 100px;
  resize: vertical;
}

input:focus,
textarea:focus,
select:focus {
  border-color: #2c3e50;
  outline: none;
}

.form-actions {
  display: flex;
  justify-content: space-between;
}
</style>
