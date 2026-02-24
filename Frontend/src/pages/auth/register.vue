<template>
	<div class="auth-page">
		<div class="auth-card">
			<div class="auth-logo">
				<el-image src="/logo.png"></el-image>
			</div>

			<h2 class="auth-title">Create account</h2>
			<p class="auth-sub">Start taking notes today</p>

			<el-form
				ref="formRef"
				:model="form"
				:rules="rules"
				label-position="top"
				@keyup.enter="handleSubmit"
			>
				<el-form-item label="Username" prop="username">
					<el-input
						v-model="form.username"
						placeholder="johndoe"
						:prefix-icon="User"
						size="large"
					/>
				</el-form-item>

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

				<el-form-item label="Confirm Password" prop="confirmPassword">
					<el-input
						v-model="form.confirmPassword"
						:type="showConfirm ? 'text' : 'password'"
						placeholder="••••••••"
						:prefix-icon="Lock"
						size="large"
					>
						<template #suffix>
							<el-icon class="eye" @click="showConfirm = !showConfirm">
								<View v-if="!showConfirm" /><Hide v-else />
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
					Create Account
				</el-button>
			</el-form>

			<p class="auth-footer">
				Already have an account?
				<router-link to="/login" class="auth-link">Sign in</router-link>
			</p>
		</div>
	</div>
</template>

<script setup lang="ts">
import { register } from "@/services/auth";
import { useStore } from "@/stores";
import { Session } from "@/utils/storage";
import { Hide, Lock, Message, User, View } from "@element-plus/icons-vue";
import type { FormInstance, FormRules } from "element-plus";
import { ElMessage } from "element-plus";
import { reactive, ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const formRef = ref<FormInstance>();
const loading = ref(false);
const show = ref(false);
const showConfirm = ref(false);

const form = reactive({
	username: "",
	email: "",
	password: "",
	confirmPassword: "",
});
const store = useStore();
const validateConfirm = (
	_: unknown,
	value: string,
	callback: (e?: Error) => void
) => {
	if (value !== form.password) {
		callback(new Error("Passwords do not match"));
	} else {
		callback();
	}
};

const rules: FormRules = {
	username: [
		{ required: true, message: "Username is required", trigger: "blur" },
		{ min: 3, message: "At least 3 characters", trigger: "blur" },
	],
	email: [
		{ required: true, message: "Email is required", trigger: "blur" },
		{ type: "email", message: "Enter a valid email", trigger: "blur" },
	],
	password: [
		{ required: true, message: "Password is required", trigger: "blur" },
		{ min: 6, message: "At least 6 characters", trigger: "blur" },
	],
	confirmPassword: [
		{
			required: true,
			message: "Please confirm your password",
			trigger: "blur",
		},
		{ validator: validateConfirm, trigger: "blur" },
	],
};

const handleSubmit = async () => {
	await formRef.value?.validate(async (valid) => {
		if (!valid) return;
		loading.value = true;
		try {
			// TODO: await authStore.register(form.username, form.email, form.password)
			const request = {
				username: form.username,
				email: form.email,
				password: form.password,
			};
			const response = await register(request);
			Session.set("token", response.token);
			store.setUserInfo({
				username: response.username,
				email: response.email,
				id: response.id,
			});
			ElMessage.success("Account created! Please sign in.");
			router.push("/dashboard");
		} catch {
			ElMessage.error("Registration failed. Try again.");
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
