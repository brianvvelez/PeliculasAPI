import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || '/api',
  headers: { 'Content-Type': 'application/json' },
});

// Género
export const generoService = {
  getAll: () => api.get('/genero'),
  getActivos: () => api.get('/genero/activos'),
  getById: (id) => api.get(`/genero/${id}`),
  create: (data) => api.post('/genero', data),
  update: (id, data) => api.put(`/genero/${id}`, data),
  delete: (id) => api.delete(`/genero/${id}`),
};

// Director
export const directorService = {
  getAll: () => api.get('/director'),
  getActivos: () => api.get('/director/activos'),
  getById: (id) => api.get(`/director/${id}`),
  create: (data) => api.post('/director', data),
  update: (id, data) => api.put(`/director/${id}`, data),
  delete: (id) => api.delete(`/director/${id}`),
};

// Productora
export const productoraService = {
  getAll: () => api.get('/productora'),
  getActivas: () => api.get('/productora/activas'),
  getById: (id) => api.get(`/productora/${id}`),
  create: (data) => api.post('/productora', data),
  update: (id, data) => api.put(`/productora/${id}`, data),
  delete: (id) => api.delete(`/productora/${id}`),
};

// Tipo
export const tipoService = {
  getAll: () => api.get('/tipo'),
  getById: (id) => api.get(`/tipo/${id}`),
  create: (data) => api.post('/tipo', data),
  update: (id, data) => api.put(`/tipo/${id}`, data),
  delete: (id) => api.delete(`/tipo/${id}`),
};

// Media
export const mediaService = {
  getAll: () => api.get('/media'),
  getById: (id) => api.get(`/media/${id}`),
  getBySerial: (serial) => api.get(`/media/serial/${serial}`),
  getByGenero: (generoId) => api.get(`/media/por-genero/${generoId}`),
  create: (data) => api.post('/media', data),
  update: (id, data) => api.put(`/media/${id}`, data),
  delete: (id) => api.delete(`/media/${id}`),
};

export default api;
