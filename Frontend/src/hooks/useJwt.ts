
const TOKEN_KEY = 'jwtToken'
function appendToken(config: any) {
    const token = localStorage.getItem(TOKEN_KEY)

    if (!token) {
        return
    }

    config.headers.Authorization = `Bearer ${token}`
}

function writeToken(token: string) {
    localStorage.setItem(TOKEN_KEY, token)
}

function removeToken() {
    localStorage.removeItem(TOKEN_KEY)
}

export { appendToken, removeToken, writeToken }
