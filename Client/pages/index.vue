<template>
  <div>
    Open bugs
    <v-card
      v-for="bug in bugs"
      :key="bug.id"
      :to="{ name: 'bugs-slug', params: { slug: bug.slug } }"
    >
      <v-card-title class="headline">{{ bug.title }}</v-card-title>
    </v-card>
  </div>
</template>

<script>
export default {
  components: {},
  asyncData() {
    return {
      bugs: [
        { slug: 1, title: 'foo' },
        { slug: 2, title: 'bar' },
      ],
    }
  },
}
</script>
