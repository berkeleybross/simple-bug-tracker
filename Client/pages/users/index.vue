<template>
  <div>
    Users
    <v-card
      v-for="user in users"
      :key="user.id"
      :to="{ name: 'users-edit-id', params: { id: user.id } }"
    >
      <v-card-title class="headline">{{ user.name }}</v-card-title>
    </v-card>
  </div>
</template>

<script>
export default {
  async asyncData({ app }) {
    return { users: await app.$http.$get('api/users') }
  },
}
</script>
