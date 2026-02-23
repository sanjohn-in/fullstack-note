import { Session } from '@/utils/storage';
import axios, { type AxiosInstance } from 'axios';
import { ElMessage, ElMessageBox } from 'element-plus';
// Configure a new axios instance

const service: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  timeout: 50000,
  headers: { 'Content-Type': 'application/json' },

  withCredentials: true,
});

// Add request interceptor
service.interceptors.request.use(
  (config) => {
    // What to do before sending a request token
    if (Session.get('token')) {
      config.headers!['Authorization'] = `${Session.get('token')}`;
    }
    return config;
  },
  (error) => {
    // What to do about request errors
    return Promise.reject(error);
  }
);
// Add response interceptor
service.interceptors.response.use(
  (response) => {
    // Do something with the response data
    const res = response.data;
    if (res.code && res.code !== 200) {
      // `token` Expired or the account has been logged in elsewhere
      if (res.code === -10004 || res.code === -20004) {
        Session.clear(); // Clear all temporary browser caches
        let msgTitle = 'Hint';
        let msgText = 'You have been logged out, please log in again';

        ElMessageBox.alert(msgText, msgTitle, {})
          .then(() => {
            window.location.href = '/';
          })
          .catch(() => { });
      } else if (res.code === -10001) {
        ElMessage.error(res.message);
      }
      // return Promise.reject(service.interceptors.response)
      // ElMessage.error(response.error);
      return response.data;
    } else {
      return res;
    }
  },
  (error) => {
    // Do something about the response error
    if (error.message.indexOf('timeout') != -1) {
      ElMessage.error('network timeout');
    } else if (error.message == 'Network Error') {
      ElMessage.error('Network connection error');
    } else {
      // if (error.response.data) ElMessage.error(error.response.statusText);
      // else ElMessage.error('Interface path not found');
    }
    return Promise.reject(error);
  }
);

// Export axios instance
export default service;
