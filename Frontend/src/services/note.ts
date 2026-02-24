import requst from "@/utils/request";

export function getNotes(userId: number, params: object): Promise<EmptyObjectType> {
    return requst({
        url: `/Notes/${userId}`,
        method: 'get',
        params,
    });
}
export function createNote(userId: number, param: object): Promise<EmptyObjectType> {
    return requst.post(`/Notes/${userId}`, param);
}
export function getNote(userId: number, id: number): Promise<EmptyObjectType> {
    return requst.get(`/Notes/${userId}/${id}`);
}
export function updateNote(userId: number, id: number, param: object): Promise<EmptyObjectType> {
    return requst.put(`/Notes/${userId}/${id}`, param);
}
export function deleteNote(userId: number, id: number): Promise<EmptyObjectType> {
    return requst.delete(`/Notes/${userId}/${id}`);
}
