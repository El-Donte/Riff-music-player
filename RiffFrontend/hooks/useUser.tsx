import { User, Envelope } from "@/types";
import { createContext, useContext, useState, ReactNode, useEffect } from "react";
import { useLike } from "./useLikes";

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

    const { fetchUserLikesIds } = useLike();

    useEffect(() => {
        loadUser().finally(() => setIsLoading(false));
    }, []);

    const loadUser = async () => {
        const response = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/user`, {
            credentials: "include",
        });

        const envelope = (await response.json()) as Envelope<User>;
        
        if (envelope.errors && envelope.errors.length > 0) {
            setUser(null);
            setSession(null);
        }
    
        setUser(envelope.result);
        setSession("logged-in");

        if (envelope.result?.id) {
            await fetchUserLikesIds(envelope.result?.id);
        }
    };

    const login = async (email: string, password: string) => {
        setIsLoading(true);
        const response = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/user/login`, {
            method: "POST",
            headers: { 
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ email, password }),
            credentials: "include",
        });

        const envelope = (await response.json()) as Envelope<string>;

        if (envelope.errors && envelope.errors.length > 0) {
            setIsLoading(false);
            throw envelope.errors
        }
        
        await loadUser();
        setIsLoading(false);
    };

    const register = async ( name: string, email: string, password: string) => {
        const formData = new FormData();
        formData.append("Name", name);
        formData.append("Email", email);
        formData.append("Password", password);

        const response = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/user/register`, {
            method: "POST",
            body: formData,
        });

        const envelope = (await response.json()) as Envelope<string>;

        if (envelope.errors && envelope.errors.length > 0) {
            throw envelope.errors;
        }
    };

    const logout = async () => {
        await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/user/logout`, {
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