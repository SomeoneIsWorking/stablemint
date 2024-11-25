<template>
  <div class="url-shortener">
    <h1>URL Shortener</h1>

    <div v-if="error" class="error-message">{{ error }}</div>

    <form @submit.prevent="shortenUrl" class="url-form">
      <input
        v-model="longUrl"
        type="url"
        placeholder="Enter URL to shorten"
        required
        :disabled="isShortening"
      />
      <button type="submit" :disabled="isShortening">
        {{ isShortening ? "Shortening..." : "Shorten" }}
      </button>
    </form>

    <div v-if="urls.length" class="url-list">
      <h2>Your Shortened URLs</h2>
      <div v-if="isLoading" class="loading">Loading URLs...</div>
      <div v-else v-for="url in urls" :key="url.id" class="url-item">
        <div>
          <a :href="'/link/' + url.shortId" target="_blank"
            >/link/{{ url.shortId }}</a
          >
          <span class="original-url">({{ url.longUrl }})</span>
        </div>
        <div class="clicks">Clicks: {{ url.clicks }}</div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from "vue";
import axios from "axios";

interface ShortLink {
  id: number;
  shortId: string;
  longUrl: string;
  clicks: number;
  createdAt: string;
  userId: string;
}

const longUrl = ref("");
const urls = ref<ShortLink[]>([]);
const isLoading = ref(false);
const isShortening = ref(false);
const error = ref<string | null>(null);

const fetchUrls = async () => {
  isLoading.value = true;
  error.value = null;
  try {
    const { data } = await axios.get<ShortLink[]>("/api/shortlink");
    urls.value = data;
  } catch (err) {
    error.value = "Failed to fetch URLs. Please try again later.";
  } finally {
    isLoading.value = false;
  }
};

const shortenUrl = async () => {
  isShortening.value = true;
  error.value = null;
  try {
    const { data } = await axios.post<ShortLink>("/api/shortlink", {
      url: longUrl.value,
    });
    urls.value.unshift(data);
    longUrl.value = "";
  } catch (err) {
    if (axios.isAxiosError(err) && err.response?.status === 429) {
      error.value = "Too many requests. Please try again in a moment.";
    } else {
      error.value = "Failed to create short URL. Please try again.";
    }
  } finally {
    isShortening.value = false;
  }
};

onMounted(() => {
  fetchUrls();
});
</script>

<style scoped>
.url-shortener {
  max-width: 800px;
  margin: 0 auto;
  padding: 40px 20px;
  min-height: 100vh;
}

h1 {
  text-align: center;
  color: #2c3e50;
  margin-bottom: 2rem;
  font-size: 2.5rem;
  font-weight: 700;
}

.url-form {
  display: flex;
  gap: 12px;
  margin-bottom: 30px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  padding: 20px;
  border-radius: 12px;
  background: white;
}

.url-form input {
  flex: 1;
  padding: 12px 16px;
  border: 2px solid #e2e8f0;
  border-radius: 8px;
  font-size: 1rem;
  transition: all 0.3s ease;
}

.url-form input:focus {
  border-color: #4299e1;
  outline: none;
  box-shadow: 0 0 0 3px rgba(66, 153, 225, 0.2);
}

.url-form button {
  padding: 12px 24px;
  background: #4299e1;
  color: white;
  border: none;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
}

.url-form button:hover:not(:disabled) {
  background: #3182ce;
  transform: translateY(-1px);
}

.url-form button:disabled {
  background: #a0aec0;
  cursor: not-allowed;
}

.url-list {
  background: white;
  border-radius: 12px;
  padding: 20px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.url-list h2 {
  color: #2d3748;
  margin-bottom: 1.5rem;
  font-size: 1.5rem;
  font-weight: 600;
}

.url-item {
  display: flex;
  justify-content: space-between;
  padding: 16px;
  border-bottom: 1px solid #e2e8f0;
  transition: background-color 0.2s ease;
}

.url-item:hover {
  background-color: #f7fafc;
}

.url-item:last-child {
  border-bottom: none;
}

.url-item a {
  color: #4299e1;
  text-decoration: none;
  font-weight: 500;
  transition: color 0.2s ease;
}

.url-item a:hover {
  color: #3182ce;
}

.original-url {
  color: #718096;
  margin-left: 12px;
  font-size: 0.9rem;
}

.clicks {
  color: #4a5568;
  font-weight: 500;
  text-wrap: nowrap;
}

.error-message {
  background: #fff5f5;
  color: #c53030;
  padding: 12px;
  border-radius: 8px;
  margin-bottom: 1rem;
  border: 1px solid #feb2b2;
}

.loading {
  text-align: center;
  padding: 2rem;
  color: #718096;
  font-weight: 500;
}
</style>
