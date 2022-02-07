<template>
  <div id="app" >
    
    <nav class="sticky-top navbar navbar-expand-lg navbar-dark bg-dark ">
        <a class="navbar-brand ms-3" @click="activateTabColor(3)"> <router-link to="/" class="navbar-brand" > Navbar </router-link></a>
    </nav>

    <div class="row" >
      <div class="col-2 bg-light text-dark h-100 position-fixed">
        <router-link  to="/products"  class="navbar-brand " v-bind:class="{'text-dark': this.tabs[0].state }"> 
          <div class="row mt-2 mx-2"  v-on:click="activateTabColor(0)" > 
              <span> Product </span> 
          </div>
        </router-link>
        
        <router-link to="/brands" class="navbar-brand " v-bind:class="{'text-dark': this.tabs[1].state }"> 
          <div class="row mx-2"  v-on:click="activateTabColor(1)">
            <span > Brand </span> 
          </div>
        </router-link>
        <router-link to="/leads" class="navbar-brand " v-bind:class="{'text-dark': this.tabs[2].state }">  
          <div class="row mx-2" v-on:click="activateTabColor(2)"> 
            <span > Requests </span> 
          </div>
          </router-link> 
      </div>
      <div class="col mt-4 offset-2">
          ROBA DA AGGIUNGERE
        <router-view
        />
      </div>
    </div>

  </div>
</template>

<script>

export default {
  name: 'App',
  data(){
    return{
      tabs : [{name:"product" , state:true},{name:"brand",state:true},{name:"leads", state:true},{name:"navbar",state:false}]
    }
  },
  methods:{
    activateTabColor(n){
      for (let x in this.tabs)
        this.tabs[x].state = true
      this.tabs[n].state = false
    },

  },
  components: {
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
}
</style>
