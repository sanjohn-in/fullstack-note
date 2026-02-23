import { defineStore } from 'pinia';
/**
 * User Info
 * @methods useStore Set user information
 */
export const useStore = defineStore('store', {
    state: () => ({
        expDate: '',
        userInfo: {} as EmptyObjectType,
    }),
    actions: {
        setExpDate(date: string) {
            this.expDate = date;
        },

        setUserInfo(payload: EmptyObjectType) {
            this.userInfo = payload;
        },
    },
    persist: true,
});
