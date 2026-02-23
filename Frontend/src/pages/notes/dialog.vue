<template>
	<div class="system-user-dialog-container">
		<el-dialog
			v-model="state.dialog.isShowDialog"
			:title="state.dialog.title"
			destroy-on-close
			@close="resetFields"
		>
			<template #default>
				<el-form
					class="p-5"
					ref="formRef"
					:model="state.form"
					:rules="rules"
					label-position="left"
					label-width="120px"
				>
					<el-row :gutter="20">
						<!-- Title -->
						<el-col :xs="24" :md="24">
							<el-form-item prop="Title" label="Title">
								<el-input v-model="state.form.Title" type="text" />
							</el-form-item>
						</el-col>

						<!-- Content -->
						<el-col :xs="24" :md="24">
							<el-form-item prop="Content" label="Content">
								<el-input v-model="state.form.Content" type="textarea" />
							</el-form-item>
						</el-col>
					</el-row>
				</el-form>
			</template>

			<template #footer>
				<div class="dialog-footer">
					<el-button type="primary" @click="submitProcess">
						<div class="flex items-center gap-2">
							<el-icon>
								<Check />
							</el-icon>
							Save
						</div>
					</el-button>
				</div>
			</template>
		</el-dialog>
	</div>
</template>
<script setup lang="ts">
import { createNote } from "@/services/note";
import { Check } from "@element-plus/icons-vue";
import type { FormInstance, FormRules } from "element-plus";
import { reactive, ref } from "vue";

const state = reactive({
	form: {
		id: null,
		Title: "",
		Content: "",
	},
	dialog: {
		title: "",
		submit: "",
		cancel: "",
		isShowDialog: false,
		type: "",
	},
	loading: false,
});
const formRef = ref<FormInstance>();

const rules: FormRules = {
	Title: [
		{ required: true, message: "Title is required", trigger: "blur" },
		{ min: 3, message: "At least 3 characters", trigger: "blur" },
	],
	Content: [
		{ required: true, message: "Content is required", trigger: "blur" },
		{ min: 10, message: "At least 10 characters", trigger: "blur" },
	],
};

const emit = defineEmits(["refresh"]);
const resetFields = () => {
	state.form = {
		id: null,
		Title: "",
		Content: "",
	};
};

const openDialog = async (type: string, row: object) => {
	state.dialog.type = type;
	state.dialog.isShowDialog = true;
};
const submitProcess = async () => {
	await formRef.value?.validate(async (valid) => {
		if (!valid) return;
		try {
			state.loading = true;
			const request = {
				id: state.form.id,
				Title: state.form.Title,
				Content: state.form.Content,
			};
			const response = await createNote(request);
			state.dialog.isShowDialog = false;
			resetFields();
			ElMessage.success("Note saved successfully");
			console.log(response);
			emit("refresh", request);
		} catch (e) {
			ElMessage.error("Failed to save note");
			console.log(e);
			//TODO handle the exception
		} finally {
			state.loading = false;
		}
	});
};
defineExpose({
	openDialog,
});
</script>
