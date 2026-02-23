import requst from "@/utils/request";

// Login
export function login(param: object): Promise<EmptyObjectType> {
  return requst.post("/Auth/Login", param);
}
export function register(param: object): Promise<EmptyObjectType> {
  return requst.post("/Auth/Register", param);
}
