import { Session } from "@/utils/storage"

const TOKEN_KEY = 'token'
function appendToken(config: any) {
    const token = Session.get(TOKEN_KEY)

    if (!token) {
        return
    }

    config.headers.Authorization = `Bearer ${token}`
}

function writeToken(token: string) {
    Session.set(TOKEN_KEY, token)
}

function removeToken() {
    Session.remove(TOKEN_KEY)
}

export { appendToken, removeToken, writeToken }
