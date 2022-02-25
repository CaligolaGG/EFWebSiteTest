<template>
<div class="container" v-if="!loading">
    <form  id="insert" v-on:submit.prevent="submitForm()">
      <h3 >  {{whatPage}}  Product </h3>

      <div class="form-group my-2 mt-3">
        <div v-bind:class="{'invalid-feedback':validName,'text-danger':true}">
            Name must be longer than 1 character and not contain only spaces.
        </div>
        <input placeholder="Name" required type="text" name="pname" id="" class="form-control bg-light"  maxlength="50" v-model="form.product.name">
      </div>
      <div class="form-group mb-2">
        <textarea placeholder="Description" class="form-control bg-light"   maxlength="50" name="desc" v-model="form.product.description" style="resize:none;" rows=5></textarea>
      </div>
      <div class="form-group mb-2">
        <input placeholder=ShortDescription type="textarea" class="form-control bg-light"   maxlength="20" name="sdesc" v-model="form.product.shortDescription">
      </div>

      <div class="row">
        <div class="row">
          <div class="col-4"></div>
          <div v-bind:class="{'invalid-feedback':validBrand,'text-danger':true, 'col':true}">
            Select a brand dummy
          </div>
        </div>

        <div class="row">
          <div class="form-group mb-2 col-4">
            <div class="input-group">
              <label for="price" class="input-group-text">Price: </label>
              <input type="number" step=".01"  min="0.01" class="form-control bg-light" name="price" v-model="form.product.price">
            </div>
          </div>
          <div class="col">
            <div class="input-group">
              <label for="brands" class="input-group-text"> Brand: </label> 
              <select  required class="form-select bg-light " v-model="form.product.brandId" name="brands">
                <option  default value="0"> Select a Brand</option>
                <option v-for="brand in this.brands" :key="brand.id" v-bind:value="brand.id" > {{brand.brandName}}</option>  
              </select> 
            </div> 
          </div>
          <div class="row my-2"> <b> Categories </b></div>
          <div class="mx-2 row">
            <div v-for="cat in this.categories" :key="cat.Id"  class="form-check col-4">
              <input type="checkbox"  v-model="form.categories" v-bind:value="cat.id"  class="form-check-input">
              <label> {{cat.name}} </label> 
            </div>
          </div>
        </div>
      </div>
      <button @keyup.enter="submitForm()" type="submit" class="btn btn-primary my-2">Submit</button>
    </form>
    </div>
</template>


<script>


import Repository from "../../Api/RepoFactory";
import Utilities from "../../Utilities/utilityFunctions.js";
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
                        brandId:"0",
                        name:"",
                    },
                categories:[],
                brandName:"",
            },

            validName:true, //used to check if the product name is valid or not
            validBrand:true,
            error:false,
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
          
            let cats = await CategoriesRepository.get();
            let temp3 = await BrandRepository.get();
            this.brands = temp3.data
            this.categories = cats.data;
            this.loading = false;
        },
        //update a  product in the db by calling the specific repository function
        async updateProduct (){
            //let newProduct = { Product : this.form.product, Categories : this.form.categories }
            let newProduct = {
                Id: this.form.product.id,
                Description: this.form.product.description,
                ShortDescription: this.form.product.shortDescription,
                Price: this.form.product.price,
                BrandId: this.form.product.brandId,
                Name: this.form.product.name,
                Categories: this.form.categories,
                BrandName: this.form.brandName
            }
            console.log(newProduct)
            this.productId=await ProductsRepository.update(newProduct);
        },

        //insert a new product in the db by calling the specific repository function.
        // A different api gets called if no categories are selected for the product to insert
        async insertProduct(){
          if( !this.form.categories.length  ||this.form.categories.length == 0)
            this.form.categories = []
          //let productWithCats = {Product : this.form.product,  Categories : this.form.categories}
            let productWithCats = {
                Id: this.form.product.id,
                Description: this.form.product.description,
                ShortDescription: this.form.product.shortDescription,
                Price: this.form.product.price,
                BrandId: this.form.product.brandId,
                Name: this.form.product.name,
                Categories: this.form.categories,
                BrandName: this.form.brandName
            }
          this.productId  = await ProductsRepository.createWithCats(productWithCats)
        },
        //submit the form info. calls the specific method (update or insert) based on the id frome the route
        async submitForm(){
            this.error = false
            this.validName=true
            this.validBrand=true

            if(Utilities.isStringInvalid(this.form.product.name))
            {
                this.validName = false
                this.error = true
                window.scroll(0,0)
            }

            if(this.form.product.brandId ==0)
            {
              this.error = true
              this.validBrand = false
            }

            if(!this.error)
            {
              this.productId != 0 ? await this.updateProduct() : await this.insertProduct()
              this.$router.push({path:'/products/'+this.productId.data})
            }
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


<style scoped>
  .form-control::placeholder { /* Chrome, Firefox, Opera, Safari 10.1+ */
            color: rgba(8, 8, 8, 0.829);
            opacity: 1; /* Firefox */
}
</style>