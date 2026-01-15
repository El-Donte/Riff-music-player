"use client";

import { useUser } from "@/hooks/useUser";
import { useRouter } from "next/navigation";
import { toast } from 'react-hot-toast';
import Image from "next/image";
import { User, LogOut, Settings, Heart, Clock, ListMusic } from "lucide-react";
import usePlayer from "@/hooks/usePlayer";

export default function AccountPage() {
  const router = useRouter();
  const player = usePlayer();
  const { user, isLoading, logout } = useUser();

  const handleLogOut = () => {
      logout();
      player.reset();
      
      if (!user) {
          toast.error("Ошибка");
      } else {
          toast.success("Вы вышли из аккаунта");
      }
      router.push('/');
      router.refresh();
  };

  if (isLoading) {
    return (
      <div className="min-h-[70vh] flex items-center justify-center">
        <div className="w-10 h-10 rounded-full border-4 border-violet-500 border-t-transparent animate-spin" />
      </div>
    );
  }

  if (!user) return null;

  return (
    <div className="min-h-screen bg-gradient-to-b from-zinc-950 via-zinc-900 to-black pb-20">
      <div className="relative h-64 md:h-80 overflow-hidden">
          <div className="absolute inset-0 bg-gradient-to-t from-black via-black/60 to-transparent z-10" />
          <div className="absolute inset-0 bg-profile-header z-0" />
        <div className="relative z-20 container mx-auto px-6 pt-20 md:pt-28 flex flex-col md:flex-row items-center md:items-end gap-6 h-full">
          <div className="relative w-32 h-32 md:w-44 md:h-44 rounded-full overflow-hidden ring-4 ring-violet-500/30 shadow-2xl shadow-violet-900/40">
            {user.avatarUrl ? (
              <Image
                src={user.avatarUrl}
                alt={user.name || "User"}
                fill
                className="object-cover"
                sizes="(max-width: 768px) 128px, 176px"
              />
            ) : (
              <div className="w-full h-full bg-gradient-to-br from-violet-600 to-fuchsia-600 flex items-center justify-center text-5xl font-bold text-white">
                {user.name?.[0]?.toUpperCase() || "?"}
              </div>
            )}
          </div>

          <div className="text-center md:text-left">
            <h1 className="text-3xl md:text-5xl font-bold text-white tracking-tight">
              {user.name || "Пользователь"}
            </h1>
            <p className="text-zinc-400 mt-1.5 text-lg">{user.email}</p>
          </div>
        </div>
      </div>

      <div className="container mx-auto px-6 py-12 max-w-5xl">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          <div className="md:col-span-2 space-y-8">
            <div className="bg-zinc-900/70 backdrop-blur-sm border border-zinc-800/50 rounded-xl p-7">
              <h2 className="text-2xl font-semibold text-white mb-6 flex items-center gap-3">
                <User size={24} className="text-violet-400" />
                О себе
              </h2>
              <p className="text-zinc-300 leading-relaxed">
                Пока здесь пусто... Расскажите о себе в будущем обновлении профиля!
              </p>
            </div>

            <div className="grid grid-cols-2 sm:grid-cols-3 gap-4">
              <ActionCard icon={<Heart />} title="Любимые треки" count="128" />
              <ActionCard icon={<Clock />} title="Недавно прослушано" count="47" />
              <ActionCard icon={<ListMusic />} title="Плейлисты" count="12" />
            </div>
          </div>

          <div className="space-y-6">
            <div className="bg-zinc-900/70 backdrop-blur-sm border border-zinc-800/50 rounded-xl p-6">
              <h3 className="text-lg font-medium text-white mb-5">Настройки</h3>
              <div className="space-y-3 text-sm">
                <SettingItem icon={<Settings size={18} />} text="Редактировать профиль" />
                <SettingItem icon={<LogOut size={18} />} text="Выйти" onClick={handleLogOut} danger />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

function ActionCard({
  icon,
  title,
  count,
}: {
  icon: React.ReactNode;
  title: string;
  count: string;
}) {
  return (
    <div className="bg-zinc-900/60 border border-zinc-800/50 rounded-xl p-5 hover:bg-zinc-800/70 transition-colors group cursor-pointer">
      <div className="text-violet-400 mb-3 group-hover:text-violet-300 transition-colors">
        {icon}
      </div>
      <div className="text-sm text-zinc-400 group-hover:text-zinc-300 transition-colors">
        {title}
      </div>
      <div className="text-2xl font-bold text-white mt-1">{count}</div>
    </div>
  );
}

function SettingItem({
  icon,
  text,
  onClick,
  danger = false,
}: {
  icon: React.ReactNode;
  text: string;
  onClick?: () => void;
  danger?: boolean;
}) {
  return (
    <button
      onClick={onClick}
      className={`flex items-center gap-3 w-full p-2.5 rounded-lg text-left transition-colors
        ${danger ? "hover:bg-red-950/40 text-red-300" : "hover:bg-zinc-800/70 text-zinc-300"}`}
    >
      <span className={`${danger ? "text-red-400" : "text-zinc-400"}`}>{icon}</span>
      <span>{text}</span>
    </button>
  );
}