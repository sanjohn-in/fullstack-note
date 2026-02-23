<template>
	<div class="auth-page">
		<div class="auth-card">
			<div class="auth-logo">
				<el-image src="/logo.png"></el-image>
			</div>

			<h2 class="auth-title">Sign in</h2>
			<p class="auth-sub">Welcome back to Techbodia Note</p>

			<el-form
				ref="formRef"
				:model="form"
				:rules="rules"
				label-position="top"
				@keyup.enter="handleSubmit"
			>
				<el-form-item label="Email" prop="email">
					<el-input
						v-model="form.email"
						placeholder="you@example.com"
						:prefix-icon="Message"
						size="large"
					/>
				</el-form-item>

				<el-form-item label="Password" prop="password">
					<el-input
						v-model="form.password"
						:type="show ? 'text' : 'password'"
						placeholder="••••••••"
						:prefix-icon="Lock"
						size="large"
					>
						<template #suffix>
							<el-icon class="eye" @click="show = !show">
								<View v-if="!show" /><Hide v-else />
							</el-icon>
						</template>
					</el-input>
				</el-form-item>

				<el-button
					type="primary"
					size="large"
					class="auth-btn"
					:loading="loading"
					@click="handleSubmit"
				>
					Sign In
				</el-button>
			</el-form>

			<p class="auth-footer">
				Don't have an account?
				<router-link to="/register" class="auth-link">Create one</router-link>
			</p>
		</div>
	</div>
</template>

<script setup lang="ts">
import { login } from "@/services/auth";
import { useStore } from "@/stores/index";
import { Session } from "@/utils/storage";
import { Hide, Lock, Message, View } from "@element-plus/icons-vue";
import type { FormInstance, FormRules } from "element-plus";
import { ElMessage } from "element-plus";
import { reactive, ref } from "vue";
import { useRouter } from "vue-router";
const store = useStore();

const router = useRouter();
const formRef = ref<FormInstance>();
const loading = ref(false);
const show = ref(false);

const form = reactive({
	email: "admin@gmail.com",
	password: "123456",
});

const rules: FormRules = {
	email: [
		{ required: true, message: "Email is required", trigger: "blur" },
		{ type: "email", message: "Enter a valid email", trigger: "blur" },
	],
	password: [
		{ required: true, message: "Password is required", trigger: "blur" },
		{ min: 6, message: "At least 6 characters", trigger: "blur" },
	],
};

const handleSubmit = async () => {
	await formRef.value?.validate(async (valid) => {
		if (!valid) return;
		loading.value = true;
		try {
			const request = {
				email: form.email,
				password: form.password,
			};
			const response = await login(request);
			Session.set("token", response.token);
			store.setUserInfo({
				username: response.username,
				email: response.email,
			});
			ElMessage.success("Welcome back!");
			router.push("/dashboard");
		} catch (error) {
			ElMessage.error("Invalid email or password");
		} finally {
			loading.value = false;
		}
	});
};
</script>

<style scoped>
.auth-page {
	min-height: 100vh;
	display: flex;
	align-items: center;
	justify-content: center;
	background: #f5f7fa;
}
.auth-card {
	width: 100%;
	max-width: 400px;
	margin: 1rem;
	padding: 2.5rem;
	background: #fff;
	border-radius: 16px;
	box-shadow: 0 4px 24px rgba(0, 0, 0, 0.08);
}
.auth-logo {
	width: 46px;
	height: 46px;
	/* background: linear-gradient(135deg, #4f6ef7, #7c3aed); */
	border-radius: 12px;
	display: flex;
	align-items: center;
	justify-content: center;
	margin-bottom: 1.25rem;
}
.auth-title {
	font-size: 1.5rem;
	font-weight: 700;
	color: #1e293b;
	margin: 0 0 4px;
}
.auth-sub {
	font-size: 0.875rem;
	color: #94a3b8;
	margin: 0 0 1.75rem;
}
.auth-btn {
	width: 100%;
	margin-top: 0.5rem;
	height: 44px;
	font-weight: 600 !important;
	border-radius: 8px !important;
	/* background: linear-gradient(135deg, #4f6ef7, #7c3aed) !important; */
	border: none !important;
}
.auth-footer {
	text-align: center;
	margin-top: 1.5rem;
	font-size: 0.875rem;
	color: #64748b;
}
.auth-link {
	color: #4f6ef7;
	font-weight: 500;
	text-decoration: none;
}
.auth-link:hover {
	text-decoration: underline;
}
.eye {
	cursor: pointer;
	color: #94a3b8;
}
.eye:hover {
	color: #4f6ef7;
}
:deep(.el-form-item__label) {
	font-weight: 500;
	color: #475569;
}
:deep(.el-input__wrapper) {
	border-radius: 8px !important;
}
</style>
