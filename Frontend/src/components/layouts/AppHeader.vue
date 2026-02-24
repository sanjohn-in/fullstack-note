<template>
	<div class="header-inner">
		<el-button text @click="$emit('update:collapsed', !collapsed)">
			<el-icon><Menu /></el-icon>
		</el-button>

		<div class="right">
			<el-switch
				v-model="dark"
				active-text="Dark"
				inactive-text="Light"
				@change="toggleTheme"
			/>

			<el-dropdown>
				<span class="user">
					{{ store.userInfo?.username.toUpperCase() }}
					<el-icon><ArrowDown /></el-icon>
				</span>
				<template #dropdown>
					<el-dropdown-menu>
						<el-dropdown-item>Profile</el-dropdown-item>
						<el-dropdown-item divided @click="onLogout"
							>Logout</el-dropdown-item
						>
					</el-dropdown-menu>
				</template>
			</el-dropdown>
		</div>
	</div>
</template>

<script setup lang="ts">
import { useStore } from "@/stores";
import { Session } from "@/utils/storage";
import { ArrowDown, Menu } from "@element-plus/icons-vue";
import { ref } from "vue";

defineProps<{ collapsed: boolean }>();
defineEmits(["update:collapsed"]);
const store = useStore();
const dark = ref(false);

const toggleTheme = () => {
	document.documentElement.classList.toggle("dark", dark.value);
};
const onLogout = () => {
	// Implement logout logic here
	Session.clear();
	console.log("Logout clicked");
	location.reload();
};
</script>

<style scoped>
.header-inner {
	width: 100%;
	display: flex;
	justify-content: space-between;
	align-items: center;
}

.right {
	display: flex;
	align-items: center;
	gap: 16px;
}

.user {
	cursor: pointer;
}
</style>
