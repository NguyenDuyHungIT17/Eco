import { API_BASE_URL } from 'app-setting'

export async function post<T>(path: string, payload: unknown): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  })

  const body = await response.json().catch(() => null) as {
    success?: boolean
    message?: string
    data?: T
    errors?: string[]
  } | null

  if (!response.ok || !body?.success || body.data === undefined) {
    throw new Error([body?.message, ...(body?.errors || [])].filter(Boolean).join(' ') || 'Yêu cầu không thành công.')
  }
  return body.data
}
