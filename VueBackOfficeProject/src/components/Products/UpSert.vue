<template>
<div class="container" v-if="!loading">
    <form  id="insert" v-on:submit.prevent="submitForm()">
      <h3 >  {{whatPage}}  Product </h3>
      <div class="form-group mb-2">
        <label for="pname">Name</label>
        <input required type="text" name="pname" id="" class="form-control"  maxlength="50" v-model="form.product.name">
      </div>
      <div class="form-group mb-2">
        <label for="desc">Description</label>
        <input type="textarea" class="form-control"  maxlength="50" name="desc" v-model="form.product.description">
      </div>
      <div class="form-group mb-2">
        <label for="sdesc">ShortDescription</label>
        <input type="textarea" class="form-control"   maxlength="20" name="sdesc" v-model="form.product.shortDescription">
      </div>
      <div class="form-group mb-2">
        <label for="price">Price</label>
        <input type="number"  min="1" class="form-control" name="price" v-model="form.product.price">
      </div>
      <div>
        BrandId 
        <select required name="" id="" class="form-select m-1 "  v-model="form.product.brandId">
          <option  value="">Please select one</option>
          <option v-for="brand in this.brands" :key="brand.id" v-bind:value="brand.id" > {{brand.name}}</option>  
        </select>  
        <div class="row my-4"> <b> Categories </b>
          <div class="mx-2">
          <li  v-for="cat in this.categories" :key="cat.Id"  class="form-check">
            <input type="checkbox"  v-model="form.categories" v-bind:value="cat.id"  class="form-check-input">
            <label> {{cat.name}} </label> 
          </li>
          </div>
          <!--
          <select name="categories" id="" class="form-select m-1" v-model="form.categories" multiple>
            <option  value="">Please select one</option>
            <option v-for="cat in this.categories" :key="cat.Id" v-bind:value="cat.id"> {{cat.name}} </option>
          </select>-->
        </div>
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
            brandSelect:"",
            productId:0,

            brands:{},//object to contains a list of brands projection with name and id
            categories: {}, //object to contains a list of cateories 
            info:{},

            form://object to hold the data inserted in the form that has to be passed to the api
            {
                product:{
                        id:0,
                        description:"",
                        shortDescription:"",
                        price:1,
                        brandId:null,
                        name:"",
                    },
                categories:[],
                brandName:"",
            },
        }
    },
    methods:{
        async getData(){
            if(this.productId != 0)
            {
                let temp= await ProductsRepository.getProduct(this.productId);
                this.form = temp.data;

                let c = this.form.categories.slice()
                this.form.categories = [];
                for(let i of c)
                    this.form.categories.push(i.id);
            }
            console.log(this.form);

            let cats = await CategoriesRepository.get();
            let temp3 = await BrandRepository.get();
            this.brands = temp3.data
            this.categories = cats.data;
            this.loading = false;
        },
        //update a  product in the db by calling the specific repository function
        async updateProduct (){
            let newProduct = { Product : this.form.product, Categories : this.form.categories }
            console.log(newProduct);
            this.productId=await ProductsRepository.update(newProduct);
            
        },
        //insert a new product in the db by calling the specific repository function.
        // A different api gets called if no categories are selected for the product to insert
        async insertProduct(){
          if(this.form.categories.length == 0)
              this.productId = await ProductsRepository.create(this.form.product)
          else
          {
              let productWithCats = {Product : this.form.product,  Categories : this.form.categories}
              this.productId  = await ProductsRepository.createWithCats(productWithCats)
          }
        },
        //submit the form info. calls the specific method (update or insert) based on the id frome the route
        async submitForm(){
            if(this.productId != 0)
                await this.updateProduct()
            else
                await this.insertProduct()
            
            console.log(this.productId)
            this.$router.push({path:'/products/'+this.productId.data.id})
        }
    },
    async created(){
      if(this.$route.params.id)
        this.productId = this.$route.params.id;
      
      await this.getData();
    },
    computed:{
      //tells if the form is for an update or an insert
      whatPage(){
        return this.productId ==0? "Insert New": "Update"
      }
    }
    
}

</script>


