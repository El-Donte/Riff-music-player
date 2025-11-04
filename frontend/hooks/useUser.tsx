import { useContext ,createContext, useState } from "react";

type UserContextType = {
    accessToken: string | null;
    user: boolean | null;
    isLoading: boolean | null;
}

export const UserContext = createContext<UserContextType | undefined>(
    undefined
);

export interface Props{
    [propName: string] : any;
};

export const MyUserContextProvider = (props: Props) => {
    const user = true;
    const [ isLoadingData, setIsLoadingData] = useState(false);
    const accessToken = "ddddaadsa";
    const value = {
        accessToken,
        user,
        isLoading: isLoadingData
    }

    return <UserContext.Provider value={value} {...props}/>
}

export const useUser = () =>{
    const context = useContext(UserContext);
    if(context === undefined){
        throw new Error('useUser should be used is MyUserContextProvider')
    }

    return context;
}