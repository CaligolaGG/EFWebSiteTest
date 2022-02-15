<template>
  <div>
    <nav class="container-fluid sticky-top navbar navbar-expand-lg navbar-dark bg-dark ">
        <a class="navbar-brand ms-3" > <router-link to="/" class="navbar-brand" > Navbar </router-link></a>
    </nav>
    <div class="container-fluid" id="app" >

      <div class="row " >
        <div class="col-2 bg-light text-dark h-100 position-fixed">
          <div class="row hoverV2">
            <router-link  to="/products"  class="navbar-brand " v-bind:class="{'text-dark': this.tabs[0].state }"> 
              <div class="row mt-2 mx-2"  > 
                  <span> Product </span> 
              </div>
            </router-link>
          </div>
          <div class="row hoverV2">
            <router-link to="/brands" class="navbar-brand " v-bind:class="{'text-dark': this.tabs[1].state }"> 
              <div class="row mx-2"  >
                <span > Brand </span> 
              </div>
            </router-link>
          </div>
          <div class="row hoverV2">
            <router-link to="/leads" class="navbar-brand " v-bind:class="{'text-dark': this.tabs[2].state }">  
              <div class="row mx-2" > 
                <span > Requests </span> 
              </div>
              </router-link> 
            </div>
        </div>
        <div class="col mt-4 offset-2">
          <router-view
          />
        </div>
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
    
    //Used to change the color of the tabs in the sidebar.
    activateTabColor(n){
      for (let x in this.tabs)
        this.tabs[x].state = true
      this.tabs[n].state = false
    },
    //changes the color of the tabs in the sidebar by checking the route name
    selectTab(){
      var check =this.$route.name
      this.activateTabColor(3)
      if (check.includes("product"))
        this.activateTabColor(0)
      if(check.includes("brand"))
        this.activateTabColor(1)
      if(check.includes("lead"))
        this.activateTabColor(2)
    },

  },
  components: {
  },
  created(){
    this.selectTab()
  },
  watch:{
    $route: function(){
    this.selectTab()}
  },

}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
}
.sortArrow {
  font-size:15px;
  padding-top: 3px;
  padding-bottom: 3px;
}
.hover:hover{
  transform: scale(1.001);
  box-shadow: 0 10px 20px rgba(0,0,0,.12), 0 4px 8px rgba(0,0,0,.06);
  cursor: pointer;
}
.hoverV2:hover{
  background-color: #757575a4;
  cursor: pointer;
}
</style>
