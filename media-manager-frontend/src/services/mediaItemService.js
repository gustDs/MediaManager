const BASE_URL = 'http://localhost:5264/api/media-items'

async function request(url, options = {}) {
  const response = await fetch(url, {
    headers: { 'Content-Type': 'application/json' },
    ...options,
  })
  if (!response.ok) {
    const error = await response.json().catch(() => ({ error: response.statusText }))
    throw new Error(error.error ?? response.statusText)
  }
  if (response.status === 204) return null
  return response.json()
}

export function getAll() {
  return request(BASE_URL)
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
