<template>
  <div>
    Open bugs
    <v-btn :to="{ name: 'bugs-new' }">New</v-btn>
    <v-card
      v-for="bug in bugs"
      :key="bug.id"
      :to="{ name: 'bugs-view-slug', params: { slug: bug.slug } }"
    >
      <v-card-title class="headline">{{ bug.title }}</v-card-title>
    </v-card>
  </div>
</template>

<script>
export default {
  components: {},
  async asyncData({ app }) {
    return { bugs: await app.$http.$get('api/bugs') }
  },
}
</script>
