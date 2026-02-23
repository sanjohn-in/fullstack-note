<template>
  <el-row :gutter="20">
    <el-col :span="6" v-for="item in stats" :key="item.label">
      <el-card shadow="hover">
        <div class="stat">
          <div class="value">{{ item.value }}</div>
          <div class="label">{{ item.label }}</div>
        </div>
      </el-card>
    </el-col>
  </el-row>
</template>

<script setup lang="ts">
const stats = [
  { label: 'Users', value: 120 },
  { label: 'Orders', value: 89 },
  { label: 'Revenue', value: '$12,300' },
  { label: 'Active', value: 42 }
]
</script>

<style scoped>
.stat {
  text-align: center;
}
.value {
  font-size: 28px;
  font-weight: bold;
}
.label {
  color: #999;
}
</style>
