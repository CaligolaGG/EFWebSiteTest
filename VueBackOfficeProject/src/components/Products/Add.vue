<template>
<div class="container">
    <form  id="insert" v-if="insert" v-on:submit.prevent="submitForm()">
      insert new product
      <div class="form-group mb-2">
        <label for="pname">Name</label>
        <input type="text" name="pname" id="" class="form-control"  maxlength="50" v-model="form.Name">
      </div>
      <div class="form-group mb-2">
        <label for="desc">Description</label>
        <input type="textarea" class="form-control"  maxlength="50" name="desc" v-model="form.Description">
      </div>
      <div class="form-group mb-2">
        <label for="sdesc">ShortDescription</label>
        <input type="textarea" class="form-control"   maxlength="20" name="sdesc" v-model="form.ShortDescription">
      </div>
      <div class="form-group mb-2">
        <label for="price">Price</label>
        <input type="number" class="form-control" name="price" v-model="form.Price">
      </div>
      <div>
        BrandId 
        <select name="" id="" class="form-select m-1 "  v-model="form.BrandId">
          <option  value="">Please select one</option>
          <option v-for="brand in this.brands" :key="brand.id" v-bind:value="brand.id" > {{brand.name}}</option>  
        </select>  
        Categories 
        <li >
          <input type="checkbox"  v-model="catsSelect" v-for="cat in this.categories" :key="cat.Id" v-bind:value="cat.id"> {{cat.name}} 
        </li>

<!--
        <select name="categories" id="" class="form-select m-1" v-model="catsSelect" multiple>
          <option  value="">Please select one</option>
          <option v-for="cat in this.categories" :key="cat.Id" v-bind:value="cat.id"> {{cat.name}} </option>
        </select>-->
      </div>
      <button type="submit" class="btn btn-primary mt-2">Submit</button>
    </form>
    </div>
</template>


<script>

import Repository from "../../Api/RepoFactory";
const ProductsRepository = Repository.get("products");

const BrandRepository = Repository.get("brands");
const CategoriesRepository = Repository.get("categories");


export default{
    data(){
        return {
            loading: true, //id of the product (from routing)
            insert:true,
            brandSelect:"",

            brands:{},//object to contains a list of brands projection with name and id
            categories: {}, //object to contains a list of cateories 

            form://object to hold the data inserted in the form that has to be passed to the api
            {
                Name:"",
                Description:"",
                ShortDescription:"",
                Price:0,
                BrandId:null,
            },
            catsSelect:[] //array to hold the list of classes selected for the product to insert 
        }
    },
    methods:{
        async getData(){
            let temp = await BrandRepository.get();
            this.brands = temp.data
            let cats = await CategoriesRepository.get();
            this.categories = cats.data;
            this.loading = false;
        },

        //insert a new product in the db by calling the specific repository function.
        // A different api gets called if no categories are selected for the product to insert
        async submitForm(){
          var id = 0;
          if(this.catsSelect.length == 0)
              id = await ProductsRepository.create(this.form)
          else
          {
              let productWithCats = {Product : this.form, Categories: this.catsSelect}
              id = await ProductsRepository.createWithCats(productWithCats)
          }
          this.$router.push({path:'/products/'+id.data.id})

        }
    },
    async created(){
      await this.getData();
    }
}
</script>


