'use client';

import Modal from "./Modal";
import useAuthModal from "@/hooks/Modals/useAuthModal";
import GoogleIcon from "@mui/icons-material/Google";

import { useRouter } from "next/navigation";
import { useUser } from "@/hooks/useUser";
import { useEffect, useState } from "react";

import {
  Box,
  TextField,
  Button,
  Typography,
  Divider,
  CircularProgress,
  Alert,
} from "@mui/material";

import useRegisterModal from "@/hooks/Modals/useRegisterModal";
import { ResponseError } from "@/types";

interface FieldErrors {
  email?: string;
  user?: string;
  password?: string;
  general?: string;
}

const AuthModal = () => {
  const { session, login, isLoading } = useUser();
  const router = useRouter();
  const { onClose, isOpen } = useAuthModal();
  const {onOpen} = useRegisterModal();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [fieldErrors, setFieldErrors] = useState<FieldErrors>({});
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
      setEmail("");
      setPassword("");
      setFieldErrors({});
      onClose();
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setFieldErrors({});
    setIsSubmitting(true);

    try {
        await login(email, password);
    } catch (errors: any) {
        if (errors.length > 0) {
            const newFieldErrors: FieldErrors = {};
            let generalMsg = "";

            errors.forEach((err: ResponseError) => {
            if (err.invalidField) {
                console.log(err)
                newFieldErrors[err.invalidField.toLowerCase() as keyof FieldErrors] = err.message;
            } else {
                
                generalMsg = err.message;
            }
            });

            setFieldErrors(newFieldErrors);
            if (generalMsg){
                
                setError(generalMsg);
            }
        }
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleGoogleLogin = () => {
    alert("Google вход скоро будет!");
  };

  return (
  <Modal
    title="С возвращением!"
    description="Войдите в свой аккаунт"
    isOpen={isOpen}
    onChange={onChange}
  >
    <Box component="form" onSubmit={handleSubmit} sx={{ mt: 2 }}>
      {error && (
        <Alert severity="error" sx={{ mb: 2, borderRadius: 2 }}>
          {error}
        </Alert>
      )}

      <TextField
        error={!!fieldErrors.email ||  !!fieldErrors.user}
        helperText={fieldErrors.email || fieldErrors.user}

        margin="normal"
        required
        fullWidth
        label="Email"
        type="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
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
        error={!!fieldErrors.password}
        helperText={fieldErrors.password}   

        margin="normal"
        required
        fullWidth
        label="Пароль"
        type="password"
        value={password}
        InputLabelProps={{ style: { color: "#e9d5ff" } }}
        InputProps={{ style: { color: "white" } }}
        onChange={(e) => setPassword(e.target.value)}
        disabled={isSubmitting || isLoading}
        sx={{
          "& .MuiOutlinedInput-root": {
            "& fieldset": { borderColor: "#a78bfa" },
            "&:hover fieldset": { borderColor: "#8b5cf6" },
            "&.Mui-focused fieldset": { borderColor: "#7c3aed" },
          },
          "& .MuiInputLabel-root": { color: "#a78bfa" },
          "& .MuiInputLabel-root.Mui-focused": { color: "#7c3aed" },
        }}
        style={{color: "white"}}
      />

      <Button
        type="submit"
        fullWidth
        variant="contained"
        disabled={isSubmitting || isLoading || !email || !password}
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
          "Войти"
        )}
      </Button>

      <Divider sx={{ my: 3 }}>
        <Typography variant="body2" color="#c4b5fd">
          или
        </Typography>
      </Divider>

      <Button
        fullWidth
        variant="outlined"
        startIcon={<GoogleIcon />}
        onClick={handleGoogleLogin}
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
          "& .MuiSvgIcon-root": {
            color: "#a78bfa",
          },
        }}
      >
        Войти через Google
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
        Нет аккаунта?{" "}
        <span
          onClick={() => {onClose(); onOpen()}}
          style={{
            color: "#c4b5fd",
            cursor: "pointer",
            fontWeight: 700,
            textDecoration: "underline",
            textUnderlineOffset: "4px",
          }}
        >
          Зарегистрироваться
        </span>
      </Typography>
    </Box>
  </Modal>
);
};

export default AuthModal;