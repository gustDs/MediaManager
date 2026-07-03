const BASE_URL = 'http://localhost:5264/api/auth'

async function request(url, body) {
  const response = await fetch(url, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  })
  if (!response.ok) {
    const error = await response.json().catch(() => ({ error: response.statusText }))
    throw new Error(error.error ?? response.statusText)
  }
  return response.json()
}

export function login(email, password) {
  return request(`${BASE_URL}/login`, { email, password })
}

export function getToken() {
  return localStorage.getItem('token')
}

export function saveToken(token) {
  localStorage.setItem('token', token)
}

export function removeToken() {
  localStorage.removeItem('token')
}

export function isAuthenticated() {
  return !!getToken()
}
