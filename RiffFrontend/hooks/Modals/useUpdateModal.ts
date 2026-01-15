import {create } from "zustand";

interface UpdateModalStore {
  isOpen: boolean;
  trackId: string;
  onOpen: (id: string) => void;
  onClose: () => void;
}

const useUpdateModal = create<UpdateModalStore>((set) => ({
  isOpen: false,
  trackId: "",
  onOpen: (id: string) => set({ isOpen: true, trackId: id }),
  onClose: () => set({ isOpen: false, trackId: "" }),
}));


export default useUpdateModal;