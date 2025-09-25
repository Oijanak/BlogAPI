<template>
  <div class="blogs">
   
    <div class="blogs-header">
      <h2>Blogs</h2>
      <button @click="openAddForm" class="btn btn-primary">
        + Add Blog
      </button>
    </div>

    <div class="filters">
  <label for="startDate">Start Date</label>
  <input id="startDate" type="date" v-model="filters.startDate" />
  <label for="endDate">End Date</label>
  <input id="endDate" type="date" v-model="filters.endDate" />
  <select id="approvedBy" v-model="filters.approvedBy">
  <option value="">--Approved By--</option>
  <option v-for="user in users" :key="user.id" :value="user.id">
    {{ user.name }}
  </option>
</select>
  <select id="createdBy" v-model="filters.createdBy">
  <option value="">--Created By--</option>
  <option v-for="user in users" :key="user.id" :value="user.id">
    {{ user.name }}
  </option>
</select>

  <select v-model="filters.approveStatus">
    <option value="" >--Approve Status--</option>
    <option value="Pending">Pending</option>
    <option value="Approved">Approved</option>
  </select>

  <select v-model="filters.activeStatus">
    <option value="">--Active Status--</option>
    <option value="Active">Active</option>
    <option value="Inactive">Inactive</option>
  </select>

  <button class="btn btn-primary" @click="fetchBlogs">Apply Filter</button>
</div>



<div class="table-responsive">
  <table class="table table-bordered table-striped">
    <thead>
      <tr>
        <th>Title</th>
        <th>Content</th>
        <th>Author</th>
        <th>Approve Status</th>
        <th>Active Status</th>
        <th>StartDate</th>
        <th>EndDate</th>
        <th>Created By</th>
        <th>Updated By</th>
        <th>Approved By</th>
        <th>Actions</th>
      </tr>
    </thead>
    <tbody>
      
      <tr v-if="loading">
        <td colspan="11" class="text-center">Loading blogs...</td>
      </tr>

     
      <tr v-else-if="blogs.length === 0">
        <td colspan="11" class="text-center">No blogs available.</td>
      </tr>

      <!-- Blog rows -->
      <tr v-else v-for="blog in blogs" :key="blog.blogId">
        <td>{{ blog.blogTitle }}</td>
        <td class="content">{{ blog.blogContent }}</td>
        <td>{{ blog.author?.authorName }}</td>
        <td :class="statusClass(blog.approveStatus)">
          {{ blog.approveStatus }}
        </td>
        <td :class="statusClass(blog.activeStatus)">
          {{ blog.activeStatus }}
        </td>
        <td>{{ formatDate(blog.startDate) }}</td>
        <td>{{ formatDate(blog.endDate) }}</td>
        <td>{{ blog.createdBy?.name || "N/A" }}</td>
        <td>{{ blog.updatedBy?.name || "N/A" }}</td>
        <td>{{ blog.approvedBy?.name || "N/A" }}</td>
        <td class="actions-cell">
          <button class="btn btn-warning btn-sm" @click="openUpdateForm(blog)">Update</button>
          <button class="btn btn-danger btn-sm" @click="deleteBlog(blog.blogId)">Delete</button>
          <button
            v-if="blog.approveStatus === 'Pending'"
            class="btn btn-success btn-sm"
            @click="approveBlog(blog.blogId)"
          >
            Approve
          </button>
        </td>
      </tr>
    </tbody>
  </table>
</div>


      
    </div>

  
    <div v-if="showForm" class="form-overlay">
      <div class="form-card">
        <h3>{{ isUpdate ? "Update Blog" : "Add Blog" }}</h3>
        <form @submit.prevent="saveBlog">
          <input v-model="form.blogTitle" type="text" placeholder="Title" required />
          <textarea v-model="form.blogContent" placeholder="Content" required></textarea>
          
          <select v-model="form.authorId" required>
            <option value=null disabled>Select Author</option>
            <option v-for="author in authors" :key="author.authorId" :value="author.authorId">
              {{ author.authorName }}
            </option>
          </select>
          <label for="startDate">StartDate</label>
          <input v-model="form.startDate"  type="date" required id="startDate"/>
          <label for="endDate">EndDate</label>
          <input v-model="form.endDate" id="endDate"  type="date" required />

          <div class="form-actions">
            <button type="submit" class="btn btn-success">Save</button>
            <button type="button" class="btn btn-secondary" @click="closeForm">Cancel</button>
          </div>
        </form>
      </div>
    </div>
 
  <div class="pagination" v-if="totalPages > 1">
  <button @click="goToPage(currentPage - 1)" :disabled="currentPage === 1">Prev</button>
  <span>Page {{ currentPage }} of {{ totalPages }}</span>
  <button @click="goToPage(currentPage + 1)" :disabled="currentPage === totalPages">Next</button>
</div>
</template>

<script setup >
import { ref, onMounted } from "vue";
import axios from "axios";
import { useAuthStore } from "../stores/auth";
import { isTokenExpired } from "../utils/jwtDecode";

const BLOG_API_URL = "http://localhost:5058/api/blogs";
const AUTHOR_API_URL = "http://localhost:5058/api/authors";

const blogs = ref([]);
const authors = ref([]);
const users = ref([]);
const showForm = ref(false);
const isUpdate = ref(false);
const currentPage = ref(1);
const pageSize = ref(6); 
const totalPages = ref(1);
const loading = ref(false);

const form = ref({
  blogTitle: "",
  blogContent: "",
  authorId: null,
  startDate: null,
  endDate: null,
});

const authStore = useAuthStore();

const api = axios.create({
  headers: {
    Authorization: `Bearer ${authStore.accessToken}`,
  },
});

function formatDate(date) {
  if (!date) return "";
  return new Date(date).toLocaleDateString();
}

const filters = ref({
  startDate: "",
  endDate: "",
  createdBy: "",
  approvedBy: "",
  approveStatus: "",
  activeStatus: ""
});

async function fetchBlogs() {
  loading.value = true;
  try {
    const res = await api.get(BLOG_API_URL, {
      params: {
        page: currentPage.value,
        limit: pageSize.value,
        startDate: filters.value.startDate || null,
        endDate: filters.value.endDate || null,
        createdBy: filters.value.createdBy || null,
        approvedBy: filters.value.approvedBy || null,
        approveStatus: filters.value.approveStatus || null,
        activeStatus: filters.value.activeStatus || null,
      },
    });
    blogs.value = res.data.data;
    totalPages.value = Math.ceil(res.data.totalSize / pageSize.value);
  } catch (err) {
    console.error("Error fetching blogs:", err);
  } finally {
    loading.value = false;
  }
}
async function fetchUsers() {
  try {
    const res = await api.get("http://localhost:5058/api/users");
    users.value = res.data.data; 
  } catch (err) {
    console.error("Error fetching users:", err);
  }
}


function goToPage(page) {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
    fetchBlogs();
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
  form.value = {
    blogTitle: "",
    blogContent: "",
    authorId: null,
    startDate: null,
    endDate: null,
  };
  isUpdate.value = false;
  showForm.value = true;
}

function openUpdateForm(blog) {
  form.value = {
    blogId: blog.blogId,
    blogTitle: blog.blogTitle,
    blogContent: blog.blogContent,
    authorId: blog.author?.authorId || null,
    startDate: blog.startDate ? blog.startDate.split("T")[0] : new Date().toISOString().split("T")[0],
    endDate: blog.endDate ? blog.endDate.split("T")[0] : new Date().toISOString().split("T")[0],
  };
  isUpdate.value = true;
  showForm.value = true;
}

function closeForm() {
  showForm.value = false;
}

async function saveBlog() {
  try {
    if (isUpdate.value) {
      await api.put(`${BLOG_API_URL}/${form.value.blogId}`, form.value);
    } else {
      await api.post(BLOG_API_URL, form.value);
    }
    await fetchBlogs();
    closeForm();
  } catch (err) {
    handleApiError(err, "add new blog");
  }
}

async function deleteBlog(id) {
  if (confirm("Are you sure you want to delete this blog?")) {
    try {
      await api.delete(`${BLOG_API_URL}/${id}`);
      await fetchBlogs();
    } catch (err) {
      handleApiError(err, "delete the blog");
    }
  }
}

async function approveBlog(id) {
  try {
    await api.patch(`${BLOG_API_URL}/${id}/approve`);
    await fetchBlogs();
  } catch (err) {
    handleApiError(err, "approve the blog");
  }
}
function handleApiError(err, action = "operation") {
  if (err.response) {
    if (err.response.status === 401) {
      alert(`Unauthorized: Please log in to ${action}.`);
    } else if (err.response.status === 403) {
      alert(`Forbidden: You don’t have permission to ${action}.`);
    } else {
      alert(`Error while trying to ${action}: ${err.response.data?.message || err.message}`);
    }
  } else {
    alert(`Network error `);
  }
}

function statusClass(status) {
  switch(status) {
    case "Pending":
      return "status-pending";
    case "Approved":
      return "status-approved";
    case "Active":
      return "status-active";
    case "Inactive":
      return "status-inactive";
    default:
      return "";
  }
}


onMounted(async() => {
  if (authStore.accessToken && isTokenExpired(authStore.accessToken)) {
      await authStore.refresh();
  }
  fetchAuthors();
  fetchUsers();
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
  margin-bottom: 20px;
}

.table {
  width: 100%;
  border-collapse: separate;
  border-spacing: 0;
  background: #fff;
  border-radius: 10px;
  overflow: hidden;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
}

.table thead {
  background: #2c3e50;
  color: #fff;
}

.table th,
.table td {
  padding: 12px 15px;
  vertical-align: middle;
  font-size: 0.95rem;
  border: 1px solid #e9e3e3;
}

.table th {
  text-align: left;
  font-weight: 600;
}

.content {
  max-width: 100px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.actions-cell {
  display: flex;
  flex-direction: column;
  gap: 10px;
  
}


.btn {
  padding: 6px 12px;
  border-radius: 6px;
  border: none;
  cursor: pointer;
  font-size: 0.9rem;
}

.btn-primary {
  background: #2c3e50;
  color: white;
}

.btn-warning {
  background: #f39c12;
  color: white;
}

.btn-danger {
  background: #e74c3c;
  color: white;
}

.btn-success {
  background: #27ae60;
  color: white;
  margin: 0px 5px;
}

.btn-secondary {
  background: #7f8c8d;
  color: white;
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

.form-card input,
.form-card textarea,
.form-card select {
  width: 100%;
  margin-bottom: 1rem;
  padding: 10px 12px;
  border: 1px solid #dcdcdc;
  border-radius: 8px;
  font-size: 0.95rem;
  transition: border 0.2s;
}

.pagination {
 
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
}

.pagination button {
  padding: 6px 12px;
  border-radius: 6px;
  border: 1px solid #ccc;
  cursor: pointer;
}

.pagination button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
.text-center {
  text-align: center;
}

.filters {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  margin-bottom: 15px;
}
.filters input, 
.filters select {
  padding: 6px;
  border-radius: 6px;
  border: 1px solid #ccc;
}
.filters label{
    padding: 6px;
  border-radius: 6px;
}
.loading {
  text-align: center;
  padding: 20px;
  font-size: 1.1rem;
  color: #2c3e50;
}
.no-data {
  text-align: center;
  padding: 20px;
  font-size: 1.1rem;
  color: #7f8c8d;
}

.status-pending {
  color: #f39c12;
  font-weight: bold;
}

.status-approved {
  color: #27ae60;
  font-weight: bold;
}

.status-active {
  color: #2c3e50;
  font-weight: bold;
}

.status-inactive {
  color: #e74c3c;
  font-weight: bold;
}

</style>
