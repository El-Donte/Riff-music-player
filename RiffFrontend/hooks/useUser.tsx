import { User, Envelope } from "@/types";
import { createContext, useContext, useState, ReactNode, useEffect } from "react";

type UserContextType = {
    user: User | null;
    session: string | null;
    isLoading: boolean;
    register: (name: string, email: string, password: string) => Promise<void>;
    login: (email: string, password: string) => Promise<void>;
    logout: () => Promise<void>;
};

export const UserContext = createContext<UserContextType | undefined>(
    undefined
);

export const MyUserContextProvider = ({ children }: { children: ReactNode }) => {
    const [user, setUser] = useState<User | null>(null);
    const [session, setSession] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        loadUser().finally(() => setIsLoading(false));
    }, []);

    const loadUser = async () => {
        const response = await fetch("http://localhost:8080/api/user", {
            credentials: "include",
        });

        const envelope = (await response.json()) as Envelope<User>;
        
        if (envelope.errors && envelope.errors.length > 0) {
            setUser(null);
            setSession(null);
        }
    
        setUser(envelope.result);
        setSession("logged-in");
    };

    const login = async (email: string, password: string) => {
        setIsLoading(true);
        const response = await fetch("http://localhost:8080/api/user/login", {
            method: "POST",
            headers: { 
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ email, password }),
            credentials: "include",
        });

        const envelope = (await response.json()) as Envelope<string>;

        if (envelope.errors && envelope.errors.length > 0) {
            console.error("API Errors:", envelope.errors);
        }

        await loadUser();
        setIsLoading(false);
    };

    const register = async ( name: string, email: string, password: string) => {
        const res = await fetch("http://localhost:8080/api/user/register", {
            method: "POST",
            credentials: "include",
            body: JSON.stringify({ name, email, password }),
        });

        if (!res.ok) {
            const error = await res.json();
            throw new Error(error.errors?.[0]?.message || "Ошибка регистрации");
        }

        await login(email, password);
    };

    const logout = async () => {
        await fetch("http://localhost:8080/api/user/logout", {
            method: "POST",
            credentials: "include",
        });

        setUser(null);
        setSession(null);
    };

    return (
        <UserContext.Provider
            value={{ user, session, isLoading, register, login, logout}}
        >
            {children}
        </UserContext.Provider>
    );
};

export const useUser = () => {
    const context = useContext(UserContext);
    if (!context) {
        throw new Error("useUser must be used within UserProvider");
    }

    return context;
};