import requst from "@/utils/request";

export function getNotes(param: object): Promise<EmptyObjectType> {
    return requst.get("/Notes", param);
}
export function createNote(param: object): Promise<EmptyObjectType> {
    return requst.post("/Notes", param);
}
export function getNote(id: number): Promise<EmptyObjectType> {
    return requst.get(`/Notes/${id}`);
}
export function updateNote(id: number, param: object): Promise<EmptyObjectType> {
    return requst.put(`/Notes/${id}`, param);
}
export function deleteNote(id: number): Promise<EmptyObjectType> {
    return requst.delete(`/Notes/${id}`);
}
