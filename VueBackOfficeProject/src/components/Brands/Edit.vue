<template>
<div class="container" v-if="!this.loading">

    <form  v-if="!this.loading" id="insert" v-on:submit.prevent="submitForm()">
            update Brand
            <div class="form-group mb-2">
                <label for="pname">Name</label>
                <input type="text" name="pname" id="" class="form-control"  maxlength="50" v-model="this.info.data.brandName" >
            </div>
            <div class="form-group mb-2">
                <label for="desc">Description</label>
                <input type="textarea" class="form-control" name="desc"  maxlength="50" v-model="this.info.data.description">
            </div>

            <button type="submit" class="btn btn-primary mt-2">Submit</button>
        </form>

</div>
</template>

<script>

import Repository from "../../Api/RepoFactory";
//const ProductsRepository = Repository.get("products");
const BrandRepository = Repository.get("brands");

export default {
    data() {
        return {
            id:0,       //id of the product (from routing)
            loading:true, //boolean to know if the data has been fetched yet
            info:{},      //object that contains the info of the brand fetched
        }    
    },
    methods:{
        //get the brand data by calling the specific api
        async getData(){ 
            this.info = await BrandRepository.getById(this.id);
            this.loading=false;
        },
        submitForm(){
            BrandRepository.update(this.info.data);
        }

    },
    async created(){
        this.id = this.$route.params.id;
        await this.getData();
    }
    
}
</script>
