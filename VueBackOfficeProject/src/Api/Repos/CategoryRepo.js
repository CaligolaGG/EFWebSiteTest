import Base from "../Base";
const resource = '/Category';

export default {
    //get all categories
    get() {
        return Base.get(`${resource}`);
    },

};