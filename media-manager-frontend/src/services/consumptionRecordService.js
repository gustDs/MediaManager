import { getToken } from './authService'

const BASE_URL = `${import.meta.env.VITE_API_URL}/api/consumption-records`
const MEDIA_ITEMS_URL = `${import.meta.env.VITE_API_URL}/api/media-items`

async function request(url, options = {}) {
  const response = await fetch(url, {
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${getToken()}`,
    },
    ...options,
  })
  if (!response.ok) {
    const body = await response.json().catch(() => null)
    if (body?.errors) {
      const messages = Object.values(body.errors).flat().join(' ')
      throw new Error(messages)
    }
    throw new Error(body?.error ?? body?.title ?? response.statusText)
  }
  if (response.status === 204) return null
  return response.json()
}

export function getAllByMediaItem(mediaItemId) {
  return request(`${MEDIA_ITEMS_URL}/${mediaItemId}/consumption-records`)
}

export function getById(id) {
  return request(`${BASE_URL}/${id}`)
}

export function create(data) {
  return request(BASE_URL, { method: 'POST', body: JSON.stringify(data) })
}

export function update(id, data) {
  return request(`${BASE_URL}/${id}`, { method: 'PUT', body: JSON.stringify(data) })
}

export function remove(id) {
  return request(`${BASE_URL}/${id}`, { method: 'DELETE' })
}
