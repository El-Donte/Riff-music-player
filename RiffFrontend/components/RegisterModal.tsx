'use client';

import { useRouter } from "next/navigation";
import Modal from "./Modal";
import useRegisterModal from "@/hooks/useRegisterModal"; // создай хук по аналогии
import { useUser } from "@/hooks/useUser";
import { useEffect, useState } from "react";
import GoogleIcon from "@mui/icons-material/Google";

import {
  Box,
  TextField,
  Button,
  Typography,
  Divider,
  CircularProgress,
  Alert,
} from "@mui/material";
import useAuthModal from "@/hooks/useAuthModal";

const RegisterModal = () => {
    const { session, register, isLoading } = useUser();
    const router = useRouter();
    const { onClose, isOpen } = useRegisterModal();
    const { onOpen } = useAuthModal();
    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);

    useEffect(() => {
        if (session) {
        router.refresh();
        onClose();
        }
    }, [session, router, onClose]);

    const onChange = (open: boolean) => {
        if (!open) {
            setError("");
            setName("");
            setEmail("");
            setPassword("");
            setConfirmPassword("");
            onClose();
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError("");
        setIsSubmitting(true);

        if (password !== confirmPassword) {
            setError("Пароли не совпадают");
            setIsSubmitting(false);
            return;
        }

        try {
            await register(name, email, password );
        } catch (err: any) {
            setError(err.message || "Ошибка регистрации");
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleGoogleRegister = () => {
        alert("Google регистрация скоро будет!");
    };

    return (
        <Modal
        title="Добро пожаловать в Riff!"
        description="Создайте аккаунт и начните слушать"
        isOpen={isOpen}
        onChange={onChange}
        >
        <Box component="form" onSubmit={handleSubmit} sx={{ mt: 2 }}>
            {error && (
            <Alert severity="error" sx={{ mb: 2, borderRadius: 2, backgroundColor: "#7f1d1d", color: "white" }}>
                {error}
            </Alert>
            )}

            <TextField
            margin="normal"
            required
            fullWidth
            label="Ваше имя"
            value={name}
            onChange={(e) => setName(e.target.value)}
            disabled={isSubmitting || isLoading}
            autoFocus
            InputLabelProps={{ style: { color: "#e9d5ff" } }}
            InputProps={{ style: { color: "white" } }}
            sx={{
                "& .MuiOutlinedInput-root": {
                "& fieldset": { borderColor: "#a78bfa" },
                "&:hover fieldset": { borderColor: "#8b5cf6" },
                "&.Mui-focused fieldset": { borderColor: "#7c3aed" },
                },
                "& .MuiInputLabel-root": { color: "#a78bfa" },
                "& .MuiInputLabel-root.Mui-focused": { color: "#7c3aed" },
            }}
            />

            <TextField
            margin="normal"
            required
            fullWidth
            label="Email"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            disabled={isSubmitting || isLoading}
            InputLabelProps={{ style: { color: "#e9d5ff" } }}
            InputProps={{ style: { color: "white" } }}
            sx={{
                "& .MuiOutlinedInput-root": {
                "& fieldset": { borderColor: "#a78bfa" },
                "&:hover fieldset": { borderColor: "#8b5cf6" },
                "&.Mui-focused fieldset": { borderColor: "#7c3aed" },
                },
                "& .MuiInputLabel-root": { color: "#a78bfa" },
                "& .MuiInputLabel-root.Mui-focused": { color: "#7c3aed" },
            }}
            />

            <TextField
            margin="normal"
            required
            fullWidth
            label="Пароль"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            disabled={isSubmitting || isLoading}
            InputLabelProps={{ style: { color: "#e9d5ff" } }}
            InputProps={{ style: { color: "white" } }}
            sx={{
                "& .MuiOutlinedInput-root": {
                "& fieldset": { borderColor: "#a78bfa" },
                "&:hover fieldset": { borderColor: "#8b5cf6" },
                "&.Mui-focused fieldset": { borderColor: "#7c3aed" },
                },
                "& .MuiInputLabel-root": { color: "#a78bfa" },
                "& .MuiInputLabel-root.Mui-focused": { color: "#7c3aed" },
            }}
            />

            <TextField
            margin="normal"
            required
            fullWidth
            label="Подтвердите пароль"
            type="password"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            disabled={isSubmitting || isLoading}
            InputLabelProps={{ style: { color: "#e9d5ff" } }}
            InputProps={{ style: { color: "white" } }}
            sx={{
                "& .MuiOutlinedInput-root": {
                "& fieldset": { borderColor: "#a78bfa" },
                "&:hover fieldset": { borderColor: "#8b5cf6" },
                "&.Mui-focused fieldset": { borderColor: "#7c3aed" },
                },
                "& .MuiInputLabel-root": { color: "#a78bfa" },
                "& .MuiInputLabel-root.Mui-focused": { color: "#7c3aed" },
            }}
            />

            <Button
            type="submit"
            fullWidth
            variant="contained"
            disabled={isSubmitting || isLoading || !name || !email || !password || !confirmPassword}
            sx={{
                mt: 3,
                mb: 2,
                py: 1.8,
                fontWeight: 600,
                textTransform: "none",
                fontSize: "1.1rem",
                background: "linear-gradient(135deg, #8b5cf6 0%, #6d28d9 100%)",
                boxShadow: "0 8px 25px rgba(139, 92, 246, 0.4)",
                color: "white",
                "&:hover": {
                background: "linear-gradient(135deg, #7c3aed 0%, #5b21b6 100%)",
                boxShadow: "0 12px 30px rgba(139, 92, 246, 0.5)",
                transform: "translateY(-2px)",
                },
                "&:disabled": {
                background: "#9ca3af",
                boxShadow: "none",
                },
                transition: "all 0.3s ease",
            }}
            >
            {isSubmitting || isLoading ? (
                <CircularProgress size={28} color="inherit" />
            ) : (
                "Создать аккаунт"
            )}
            </Button>

            <Divider sx={{ my: 3 }}>
            <Typography variant="body2" sx={{ color: "#c4b5fd", fontWeight: 500 }}>
                или
            </Typography>
            </Divider>

            <Button
            fullWidth
            variant="outlined"
            startIcon={<GoogleIcon />}
            onClick={handleGoogleRegister}
            disabled={isSubmitting || isLoading}
            sx={{
                py: 1.8,
                fontWeight: 500,
                textTransform: "none",
                fontSize: "1.05rem",
                borderColor: "#a78bfa",
                color: "#a78bfa",
                "&:hover": {
                borderColor: "#8b5cf6",
                backgroundColor: "rgba(139, 92, 246, 0.08)",
                color: "#8b5cf6",
                },
                "& .MuiSvgIcon-root": { color: "#a78bfa" },
            }}
            >
            Зарегистрироваться через Google
            </Button>

            <Typography
            variant="body2"
            align="center"
            sx={{
                mt: 3,
                color: "#e9d5ff",
                fontWeight: 500,
            }}
            >
            Уже есть аккаунт?{" "}
            <span
                onClick={() => {
                onClose();
                onOpen();
                }}
                style={{
                color: "#c4b5fd",
                cursor: "pointer",
                fontWeight: 700,
                textDecoration: "underline",
                textUnderlineOffset: "4px",
                }}
            >
                Войти
            </span>
            </Typography>
        </Box>
        </Modal>
    );
};

export default RegisterModal;