<template>
	<el-menu
		:collapse="collapsed"
		router
		background-color="#f54095"
		text-color="#fff"
		active-text-color="#409EFF"
		class="menu"
	>
		<el-menu-item index="/dashboard">
			<el-icon><Monitor /></el-icon>
			<span>Dashboard</span>
		</el-menu-item>

		<el-menu-item index="/note">
			<el-icon><Notebook /></el-icon>
			<span>Notes</span>
		</el-menu-item>

		<el-menu-item index="/#">
			<el-icon><Setting /></el-icon>
			<span>Settings</span>
		</el-menu-item>
	</el-menu>
</template>

<script setup lang="ts">
import { Monitor, Setting, Notebook } from "@element-plus/icons-vue";

defineProps<{
	collapsed: boolean;
}>();
</script>

<style scoped>
.menu {
	height: 100%;
	border-right: none;
}
</style>
