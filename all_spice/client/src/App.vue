<script setup>
import Navbar from './components/Navbar.vue';
import { RouterView } from 'vue-router';

</script>

<template>
  <header>
    <Navbar /> <!-- REVIEW: How can I make it show for some pages not for others? -->
  </header>
  <main>
    <RouterView/>
  </main>
  <footer class=" text-center">
    Crunched out by <i class="mdi mdi-account-hard-hat-outline"></i> by Shockwave
  </footer>
</template>

<style lang="scss">


</style>
