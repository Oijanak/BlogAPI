<template>
  <div class="blogs-container">
  
    <div class="blogs-header">
      <h2>Blog Management</h2>
      <button @click="openAddForm" class="btn btn-primary">
        <i class="icon-plus"></i> Add New Blog
      </button>
    </div>

    <!-- Filters Section -->
    <div class="filters-section">
      <div class="filter-group">
        <label for="startDate">Start Date</label>
        <input id="startDate" type="date" v-model="filters.startDate" class="filter-input" />
      </div>
      
      <div class="filter-group">
        <label for="endDate">End Date</label>
        <input id="endDate" type="date" v-model="filters.endDate" class="filter-input" />
      </div>
      
      <div class="filter-group">
        <label for="approvedBy">Approved By</label>
        <select id="approvedBy" v-model="filters.approvedBy" class="filter-select">
          <option value="">All Users</option>
          <option v-for="user in users" :key="user.id" :value="user.id">
            {{ user.name }}
          </option>
        </select>
      </div>
      
      <div class="filter-group">
        <label for="createdBy">Created By</label>
        <select id="createdBy" v-model="filters.createdBy" class="filter-select">
          <option value="">All Users</option>
          <option v-for="user in users" :key="user.id" :value="user.id">
            {{ user.name }}
          </option>
        </select>
      </div>
      
      <div class="filter-group">
        <label for="approveStatus">Approval Status</label>
        <select v-model="filters.approveStatus" class="filter-select">
          <option value=null>All Status</option>
          <option value="Pending">Pending</option>
          <option value="Approved">Approved</option>
        </select>
      </div>
      
      <div class="filter-group">
        <label for="activeStatus">Active Status</label>
        <select v-model="filters.activeStatus" class="filter-select">
          <option value=null>All Status</option>
          <option value="Active">Active</option>
          <option value="Inactive">Inactive</option>
        </select>
      </div>
      
      <div class="filter-actions">
        <button class="btn btn-secondary" @click="resetFilters">
          <i class="icon-reset"></i> Reset
        </button>
        <button class="btn btn-primary" @click="applyFilters">
          Apply Filters
        </button>
      </div>
    </div>

   
    <div class="table-section">
      <div class="table-info">
        <div class="entries-info">
          Showing {{ paginationInfo.start }} to {{ paginationInfo.end }} of {{ paginationInfo.total }} entries
        </div>
        <div class="entries-per-page">
          <label for="pageSize">Entries per page:</label>
          <select id="pageSize" v-model="pageSize" @change="onPageSizeChange" class="page-size-select">
            <option value="5">5</option>
            <option value="10">10</option>
            <option value="20">20</option>
            <option value="50">50</option>
          </select>
        </div>
      </div>

      <div class="table-responsive">
        <table class="data-table">
          <thead>
            <tr>
              <th>S.No.</th>
              <th @click="sortBy('blogTitle')" class="sortable">
                Title <span class="sort-icon" :class="getSortIcon('blogTitle')"></span>
              </th>
              <th>Content</th>
              <th @click="sortBy('author')" class="sortable">
                Author <span class="sort-icon" :class="getSortIcon('author.authorName')"></span>
              </th>
              <th @click="sortBy('approveStatus')" class="sortable">
                Approve Status <span class="sort-icon" :class="getSortIcon('approveStatus')"></span>
              </th>
              <th>Active Status</th>
              <th @click="sortBy('startDate')" class="sortable">
                Start Date <span class="sort-icon" :class="getSortIcon('startDate')"></span>
              </th>
              <th @click="sortBy('endDate')" class="sortable">End Date <span class="sort-icon" :class="getSortIcon('endDate')"></span></th>
              <th>Created By</th>
              <th>Approved By</th>
              <th class="actions-header">Actions</th>
            </tr>
          </thead>
          <tbody>
           
            <tr v-if="loading">
              <td colspan="10" class="loading-state">
                <div class="loading-spinner"></div>
                Loading blogs...
              </td>
            </tr>

            <!-- Empty State -->
            <tr v-else-if="blogs.length === 0">
              <td colspan="10" class="empty-state">
                <i class="icon-empty"></i>
                No blogs found matching your criteria.
              </td>
            </tr>

            <!-- Blog Data Rows -->
            <tr v-else v-for="(blog,index) in blogs" :key="blog.blogId" class="table-row">
              <td>{{ (currentPage - 1) * pageSize + index + 1 }}.</td>
              <td class="title-cell">{{ blog.blogTitle }}</td>
              <td class="content-cell">
                <div class="content-preview">{{ truncateContent(blog.blogContent) }}</div>
              </td>
              <td>{{ blog.author?.authorName || 'N/A' }}</td>
              <td>
                <span :class="['status-badge', statusClass(blog.approveStatus)]">
                  {{ blog.approveStatus }}
                </span>
              </td>
              <td>
                <span :class="['status-badge', statusClass(blog.activeStatus)]">
                  {{ blog.activeStatus }}
                </span>
              </td>
              <td>{{ formatDate(blog.startDate) }}</td>
              <td>{{ formatDate(blog.endDate) }}</td>
              <td>{{ blog.createdBy?.name || 'N/A' }}</td>
              <td>{{ blog.approvedBy?.name || 'N/A' }}</td>
              <td class="actions-cell">
                <div class="action-buttons">
                  <button @click="openUpdateForm(blog)" title="Edit">
                    <i style="color: orange;" class="fas fa-edit"></i>
                  </button>

                  <button @click="deleteBlog(blog.blogId)" title="Delete">
                    <i style="color: red;" class="fas fa-trash-alt"></i>
                  </button>

                  <button v-if="blog.approveStatus === 'Pending'" @click="approveBlog(blog.blogId)" title="Approve">
                     <i style="color: green;" class="fas fa-check-circle"></i>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div v-if="showForm" class="form-overlay">
      <div class="form-card">
        <h3>{{ isUpdate ? "Update Blog" : "Add Blog" }}</h3>
        <form @submit.prevent="saveBlog(form)">
          <input v-model="form.blogTitle" type="text" placeholder="Title" required />
          <textarea v-model="form.blogContent" placeholder="Content" required></textarea>
          
          <select v-model="form.authorId" required>
            <option value=null disabled>Select Author</option>
            <option v-for="author in authors" :key="author.authorId" :value="author.authorId">
              {{ author.authorName }}
            </option>
          </select>

          <input v-model="form.startDate"  type="date" required />
          <input v-model="form.endDate"  type="date" required />

          <div class="form-actions">
            <button type="submit" class="btn btn-success">Save</button>
            <button type="button" class="btn btn-secondary" @click="closeForm">Cancel</button>
          </div>
        </form>
      </div>
    </div>
  </div>

      <!-- Pagination Section -->
      <div class="pagination-section" v-if="totalPages > 1">
        <div class="pagination-info">
          Page {{ currentPage }} of {{ totalPages }}
        </div>
        <div class="pagination-controls">
          <button 
            @click="goToPage(1)" 
            :disabled="currentPage === 1"
            class="pagination-btn"
            title="First Page"
          >
            <i class="icon-first"></i>
          </button>
          <button 
            @click="goToPage(currentPage - 1)" 
            :disabled="currentPage === 1"
            class="pagination-btn"
            title="Previous Page"
          >
            <i class="icon-prev"></i>
          </button>
          
          <div class="page-numbers">
            <button
              v-for="page in visiblePages"
              :key="page"
              @click="goToPage(page)"
              :class="['page-btn', { active: page === currentPage }]"
            >
              {{ page }}
            </button>
            <span v-if="showEllipsis" class="page-ellipsis">...</span>
          </div>
          
          <button 
            @click="goToPage(currentPage + 1)" 
            :disabled="currentPage === totalPages"
            class="pagination-btn"
            title="Next Page"
          >
            <i class="icon-next"></i>
          </button>
          <button 
            @click="goToPage(totalPages)" 
            :disabled="currentPage === totalPages"
            class="pagination-btn"
            title="Last Page"
          >
            <i class="icon-last"></i>
          </button>
        </div>
        <div class="pagination-jump">
          <label for="jumpToPage">Go to page:</label>
          <input
            id="jumpToPage"
            type="number"
            :min="1"
            :max="totalPages"
            v-model="jumpToPage"
            @keyup.enter="jumpToSpecificPage"
            class="page-jump-input"
          />
          <button @click="jumpToSpecificPage" class="btn btn-secondary btn-sm">Go</button>
        </div>
      </div>
    </div>

  
   

</template>

<script setup>
import { ref, computed, onMounted, watch } from "vue";
import axios from "axios";
import { useAuthStore } from "../stores/auth";
import { isTokenExpired } from "../utils/jwtDecode";


const BLOG_API_URL = "http://localhost:5058/api/blogs";
const AUTHOR_API_URL = "http://localhost:5058/api/authors";
const USER_API_URL = "http://localhost:5058/api/users";

const blogs = ref([]);
const authors = ref([]);
const users = ref([]);
const showForm = ref(false);
const isUpdate = ref(false);
const currentPage = ref(1);
const pageSize = ref(10);
const totalPages = ref(1);
const loading = ref(false);
const jumpToPage = ref(1);
const sortField = ref('blogTitle');
const sortDirection = ref('asc');
const totalEntries = ref(0);

const filters = ref({
  startDate: null,
  endDate: null,
  createdBy: "",
  approvedBy: "",
  approveStatus: null,
  activeStatus: null,
  sortBy: "createdAt",  
  sortOrder: "desc"     
});

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

// Computed properties
const paginationInfo = computed(() => {
  const total = totalEntries.value;
  const start = total === 0 ? 0 : (currentPage.value - 1) * pageSize.value + 1;
  const end = Math.min(currentPage.value * pageSize.value, total);
  return { start, end, total };
});




const visiblePages = computed(() => {
  const pages = [];
  const maxVisible = 5;
  let start = Math.max(1, currentPage.value - Math.floor(maxVisible / 2));
  let end = Math.min(totalPages.value, start + maxVisible - 1);
  
  start = Math.max(1, end - maxVisible + 1);
  
  for (let i = start; i <= end; i++) {
    pages.push(i);
  }
  return pages;
});

const showEllipsis = computed(() => {
  return totalPages.value > visiblePages.value.length;
});

// Methods
function formatDate(date) {
  if (!date) return "N/A";
  return new Date(date).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  });
}

function truncateContent(content, maxLength = 100) {
  if (!content) return '';
  return content.length > maxLength ? content.substring(0, maxLength) + '...' : content;
}

function getNestedValue(obj, path) {
  return path.split('.').reduce((acc, part) => acc && acc[part], obj) || '';
}

function sortBy(field) {
  if (filters.value.sortBy === field) {
    filters.value.sortOrder = filters.value.sortOrder === "asc" ? "desc" : "asc";
  } else {
    filters.value.sortBy = field;
    filters.value.sortOrder = "asc";
  }

  fetchBlogs(); 
}

function getSortIcon(field) {
  if (filters.value.sortBy !== field) return '';
  return filters.value.sortOrder === 'asc' ? 'icon-sort-up' : 'icon-sort-down';
}
async function fetchBlogs() {
  loading.value = true;
  try {
    const res = await api.post(`${BLOG_API_URL}/getAll`, 
       {
        page: currentPage.value,
        limit: pageSize.value,
        sortBy: filters.value.sortBy,
      sortOrder: filters.value.sortOrder,
        ...filters.value
      }
    );
    blogs.value = res.data.data || [];
    totalEntries.value = res.data.totalSize || 0;  
    totalPages.value = Math.ceil(totalEntries.value / pageSize.value);
  } catch (err) {
    console.error("Error fetching blogs:", err);
    handleApiError(err, "fetch blogs");
  } finally {
    loading.value = false;
  }
}


async function fetchUsers() {
  try {
    const res = await api.get(USER_API_URL);
    users.value = res.data.data || [];
  } catch (err) {
    console.error("Error fetching users:", err);
  }
}

async function fetchAuthors() {
  try {
    const res = await api.get(AUTHOR_API_URL);
    authors.value = res.data.data || [];
  } catch (err) {
    console.error("Error fetching authors:", err);
  }
}

async function goToPage(page) {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
    await fetchBlogs();
  }
}

function jumpToSpecificPage() {
  const page = parseInt(jumpToPage.value);
  if (page >= 1 && page <= totalPages.value) {
    goToPage(page);
  }
}

function onPageSizeChange() {
  currentPage.value = 1;
  fetchBlogs();
}

function applyFilters() {
  currentPage.value = 1;
  fetchBlogs();
}

function resetFilters() {
  filters.value = {
    startDate: "",
    endDate: "",
    createdBy: "",
    approvedBy: "",
    approveStatus: "",
    activeStatus: ""
  };
  currentPage.value = 1;
  fetchBlogs();
}

function openAddForm() {
  form.value = {
    blogTitle: "",
    blogContent: "",
    authorId: null,
    startDate: new Date().toISOString().split('T')[0],
    endDate: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0],
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

async function saveBlog(formData) {
  try {
    if (isUpdate.value) {
      await api.put(`${BLOG_API_URL}/${formData.blogId}`, formData);
    } else {
      await api.post(BLOG_API_URL, formData);
    }
    await fetchBlogs();
    closeForm();
  } catch (err) {
    handleApiError(err, isUpdate.value ? "update blog" : "add new blog");
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
      alert(`Forbidden: You don't have permission to ${action}.`);
    } else {
      alert(`Error while trying to ${action}: ${err.response.data?.message || err.message}`);
    }
  } else {
    alert(`Network error while trying to ${action}`);
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

onMounted(async () => {
  if (authStore.accessToken && isTokenExpired(authStore.accessToken)) {
    await authStore.refresh();
  }
  await Promise.all([fetchAuthors(), fetchUsers(), fetchBlogs()]);
});

watch([() => filters.value, pageSize], () => {
  currentPage.value = 1;
});
</script>

<style scoped>
.blogs-container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 20px;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.blogs-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 30px;
  padding-bottom: 15px;
  border-bottom: 2px solid #e0e0e0;
}

.blogs-header h2 {
  color: #2c3e50;
  margin: 0;
  font-size: 2rem;
}

.filters-section {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 15px;
  margin-bottom: 25px;
  padding: 20px;
  background: #f8f9fa;
  border-radius: 8px;
  border: 1px solid #e0e0e0;
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.filter-group label {
  font-weight: 600;
  color: #495057;
  font-size: 0.9rem;
}

.filter-input,
.filter-select {
  padding: 8px 12px;
  border: 1px solid #ced4da;
  border-radius: 4px;
  font-size: 0.9rem;
  transition: border-color 0.2s;
}

.filter-input:focus,
.filter-select:focus {
  outline: none;
  border-color: #007bff;
  box-shadow: 0 0 0 2px rgba(0, 123, 255, 0.25);
}

.filter-actions {
  display: flex;
  gap: 10px;
  align-items: flex-end;
}

.table-section {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  overflow: hidden;
}

.table-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px 20px;
  background: #f8f9fa;
  border-bottom: 1px solid #e0e0e0;
}

.entries-info {
  color: #6c757d;
  font-size: 0.9rem;
}

.entries-per-page {
  display: flex;
  align-items: center;
  gap: 10px;
}

.page-size-select {
  padding: 5px 10px;
  border: 1px solid #ced4da;
  border-radius: 4px;
}

.data-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.9rem;
}

.data-table th {
  background: #2c3e50;
  color: white;
  padding: 10px 15px;
  text-align: left;
  font-weight: 400;
}

.data-table td {
  padding: 10px 20px;
  border-bottom: 1px solid #e0e0e0;
}

.sortable {
  cursor: pointer;
  user-select: none;
  transition: background-color 0.2s;
}

.sortable:hover {
  background-color: #34495e;
}

.sort-icon {
  margin-left: 5px;
  font-size: 0.8rem;
}


.content-cell {
  max-width: 200px;
}

.content-preview {
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.status-badge {
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 0.8rem;
  font-weight: 600;
  text-transform: uppercase;
}

.status-pending { background: #fff3cd; color: #856404; }
.status-approved { background: #d1edff; color: #004085; }
.status-active { background: #d4edda; color: #155724; }
.status-inactive { background: #f8d7da; color: #721c24; }

.actions-header {
  text-align: center;
}

.actions-cell {
  text-align: center;
}

.action-buttons {
  display: flex;
  gap: 5px;
  justify-content: center;
  
}
.action-buttons button {
  border: none;
  background: transparent;
  cursor: pointer;
  padding: 4px;   
}

.btn {
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 6px 12px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.85rem;
  transition: all 0.2s;
}

.btn:hover {
  transform: translateY(-1px);
  box-shadow: 0 2px 5px rgba(0,0,0,0.2);
}

.btn-primary { background: #515b68; color: white; }
.btn-secondary { background: #6c757d; color: white; }
.btn-success { background: #28a745; color: white; }
.btn-warning { background: #ffc107; color: #212529; }
.btn-danger { background: #dc3545; color: white; }

.btn-sm { padding: 4px 8px; font-size: 0.8rem; }

.loading-state,
.empty-state {
  text-align: center;
  padding: 40px;
  color: #6c757d;
}

.loading-spinner {
  border: 3px solid #f3f3f3;
  border-top: 3px solid #007bff;
  border-radius: 50%;
  width: 30px;
  height: 30px;
  animation: spin 1s linear infinite;
  margin: 0 auto 10px;
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

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.table-row:hover {
  background-color: #f8f9fa;
}

.pagination-section {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px;
  background: #f8f9fa;
  border-top: 1px solid #e0e0e0;
}

.pagination-controls {
  display: flex;
  align-items: center;
  gap: 5px;
}

.pagination-btn,
.page-btn {
  padding: 6px 12px;
  border: 1px solid #ced4da;
  background: white;
  cursor: pointer;
  transition: all 0.2s;
}

.pagination-btn:hover:not(:disabled),
.page-btn:hover {
  background: #007bff;
  color: white;
  border-color: #007bff;
}

.page-btn.active {
  background: #007bff;
  color: white;
  border-color: #007bff;
}

.pagination-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.page-numbers {
  display: flex;
  gap: 5px;
}

.page-ellipsis {
  padding: 6px 12px;
  color: #6c757d;
}

.pagination-jump {
  display: flex;
  align-items: center;
  gap: 10px;
}

.page-jump-input {
  width: 60px;
  padding: 4px 8px;
  border: 1px solid #ced4da;
  border-radius: 4px;
}

/* Responsive Design */
@media (max-width: 768px) {
  .blogs-container {
    padding: 10px;
  }
  
  .blogs-header {
    flex-direction: column;
    gap: 15px;
    text-align: center;
  }
  
  .filters-section {
    grid-template-columns: 1fr;
  }
  
  .table-info {
    flex-direction: column;
    gap: 10px;
    text-align: center;
  }
  
  .pagination-section {
    flex-direction: column;
    gap: 15px;
  }
  
  .action-buttons {
    flex-direction: column;
  }

  
}

.icon-plus::before { content: "+"; }
.icon-reset::before { content: "↺"; }
.icon-filter::before { content: "⚡"; }
.icon-first::before { content: "<<"; }
.icon-prev::before { content: "◀"; }
.icon-next::before { content: "▶"; }
.icon-last::before { content: ">>"; }
.icon-sort-up::before { content: "↑"; }
.icon-sort-down::before { content: "↓"; }
.icon-empty::before { content: "📭"; }
</style>