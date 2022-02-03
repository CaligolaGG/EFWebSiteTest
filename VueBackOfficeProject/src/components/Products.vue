<!--
<template>
    <span> Products </span>
</template>



<script lang="js">
    import Vue from 'vue';

    export default Vue.extend({
        data() {
            return {
                loading: false,
            };
        },
        created() {
        },
        watch: {
        },
        methods: {
        },
        mounted(){
            this.$emit("")
        }
    });
</script>
-->

<template>
  <div>

    insert new product
<form>
  <div class="form-group mb-2">
    <label for="pname">Name</label>
    <input type="text" name="pname" id="" class="form-control">
  </div>
  <div class="form-group mb-2">
    <label for="desc">Description</label>
    <input type="textarea" class="form-control" name="desc">
  </div>
  <div class="form-group mb-2">
    <label for="sdesc">ShortDescription</label>
    <input type="textarea" class="form-control" name="sdesc">
  </div>
  <div class="form-group mb-2">
    <label for="price">Price</label>
    <input type="number" class="form-control" name="price">
  </div>
  <div>
    BrandId 
    <select name="" id=""  class="form-control mb-2">
          <option value=""> V-for </option>
          <option value="">1</option>
          <option value="">2</option>
    </select>
    Categories 
    <select name="" id=""  class="form-control mb-2">
          <option value=""> V-for </option>
          <option value="">1</option>
          <option value="">2</option>
    </select>
  </div>
  <button type="submit" class="btn btn-primary mt-2">Submit</button>
  
</form>





    <br><br><br><br><br><br><br><br><br>
    <hr>
    <b-table striped hover :items="items" :fields="fields" thead-class="prova">
      <template slot="thead-top">
        <tr role="row">

          <th
            role="columnheader"
            scope="col"
            colspan="3"
            tabindex="4"
            aria-colindex="4"
            aria-sort="none"
            class=""
          >            
          </th>

        </tr>
      </template>
    </b-table>
  </div>
</template>
<script>
export default {
  data() {
    return {
      row:{

      },
      // Note 'isActive' is left out and will not appear in the rendered table
      fields: [
        {
          key: "last_name",
          sortable: true,
        },
        {
          key: "first_name",
          sortable: false,
        },
        {
          key: "age",
          label: "Person age",
          sortable: true,
          // Variant applies to the whole column, including the header and footer
          variant: "danger",
        },
      ],
      items: [
        {last_name: "test", color:"white"},
        {},
        {
          isActive: true,
          age: 40,
          first_name: "Dickerson",
          last_name: "Macdonald",
        },
        { isActive: false, age: 21, first_name: "Larsen", last_name: "Shaw" },
        { isActive: false, age: 89, first_name: "Geneva", last_name: "Wilson" },
        { isActive: true, age: 38, first_name: "Jami", last_name: "Carney" },
      ],
    };
  },
};
</script>
