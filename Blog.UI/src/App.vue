<template>
  <div id="app">
    <Navbar />
    <main>
      <router-view />
    </main>
  </div>
</template>

<script setup>
import Navbar from './components/Navbar.vue'
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  margin: 0;
  padding: 0;
  background: #f7f9fc;
  min-height: 100vh;
}

main {
  padding: 20px;
}
</style>
